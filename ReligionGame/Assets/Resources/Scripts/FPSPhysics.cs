using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FPSPhysics : MonoBehaviour
{
    #region Structs

    #region INPUT SCHEMA

    public const byte BTN_FORWARD = 1 << 1;
    public const byte BTN_BACKWARD = 1 << 2;
    public const byte BTN_LEFTWARD = 1 << 3;
    public const byte BTN_RIGHTWARD = 1 << 4;

    #endregion

    struct Inputs
    {
        readonly ushort buttons;

        public Inputs(ushort value) : this() => buttons = value;

        public bool IsUp(ushort button) => IsDown(button) == false;

        public bool IsDown(ushort button) => (buttons & button) == button;

        public static implicit operator Inputs(ushort value) => new Inputs(value);
    }

    struct InputCmd
    {
        public float DeliveryTime;
        public int LastAckedTick;
        public List<Inputs> Inputs;
    }

    struct SimulationStep
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Inputs Input;
    }

    struct Snapshot
    {
        public float DeliveryTime;
        public int Tick;
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Velocity;
        public Vector3 AngularVelocity;
    }

    #endregion

    const int BufferLength = 32;

    [SerializeField, Range(0, 1)] float RTT;
    [SerializeField, Range(0, 1)] float PACKET_LOSS;

    [SerializeField] GameObject ClientSimObject;
    [SerializeField] GameObject SmoothObject;
    [SerializeField] Transform CameraTransform;

    [SerializeField] int ClientTick;
    [SerializeField] int ClientLastAckedTick;

    Queue<Snapshot> ReceivedClientSnapshots;

    Queue<InputCmd> ReceivedServerInputs;

    SimulationStep[] SimulationSteps;

    LoadSceneParameters sceneParams = new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.Physics3D);

    Scene  ClientScene;
    PhysicsScene  ClientPhysicsScene;

    InputCmd inputCmd;

    Rigidbody ClientRb;

    [SerializeField] float RotationSpeed = 90;
    float CamRotation;

    float FixedStepAccumulator;

    Vector3 PreviousPosition;

    public void SetSimsObject(GameObject _client, GameObject _smooth, Transform _camera, float _rotationSpeed)
    {
        ClientSimObject = _client;
        SmoothObject = _smooth;
        CameraTransform = _camera;
        RotationSpeed = _rotationSpeed;
    }

    void Start()
    {
        Physics.autoSimulation = false;

        ReceivedClientSnapshots = new Queue<Snapshot>();

        SimulationSteps = new SimulationStep[BufferLength];

        ClientScene = SceneManager.LoadScene("Main", sceneParams);

        ClientPhysicsScene = ClientScene.GetPhysicsScene();

        SceneManager.MoveGameObjectToScene(ClientSimObject, ClientScene);

        ClientRb = ClientSimObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        FixedStepAccumulator += Time.deltaTime;

        while (FixedStepAccumulator >= Time.fixedDeltaTime)
        {
            FixedStepAccumulator -= Time.fixedDeltaTime;

            ClientUpdate();
        }

        float _alpha = Mathf.Clamp01(FixedStepAccumulator / Time.fixedDeltaTime);

        SmoothObject.transform.position = Vector3.Lerp(PreviousPosition, ClientSimObject.transform.position, _alpha);

        CamRotation += Input.GetAxisRaw("Mouse X") * RotationSpeed;
        CameraTransform.position = SmoothObject.transform.position;
        CameraTransform.rotation = Quaternion.Euler(0, CamRotation, 0);


        if (Input.GetKeyDown(KeyCode.V))
        {
            vsyncToggle = !vsyncToggle;
            QualitySettings.vSyncCount = vsyncToggle ? 1 : 0;
        }

        if (Time.unscaledTime > _timer)
        {
            fps = (int)(1f / Time.deltaTime);
            _timer = Time.unscaledTime + 1;
        }
    }

    int fps;
    float _timer;
    bool vsyncToggle = false;

    void ClientUpdate()
    {
        int stateSlot = ClientTick % BufferLength;

        ushort Buttons = 0;

        if (Input.GetKey(KeyCode.W)) Buttons |= BTN_FORWARD;
        if (Input.GetKey(KeyCode.S)) Buttons |= BTN_BACKWARD;
        if (Input.GetKey(KeyCode.A)) Buttons |= BTN_LEFTWARD;
        if (Input.GetKey(KeyCode.D)) Buttons |= BTN_RIGHTWARD;

        SimulationSteps[stateSlot].Input = Buttons;

        SetStateAndRollback(ref SimulationSteps[stateSlot], ClientRb);

        PreviousPosition = SimulationSteps[stateSlot].Position;

        if (UnityEngine.Random.value > PACKET_LOSS)
        {
            inputCmd.DeliveryTime = Time.time + RTT;
            inputCmd.LastAckedTick = ClientLastAckedTick;
            inputCmd.Inputs = new List<Inputs>();

            for (int tick = inputCmd.LastAckedTick; tick <= ClientTick; ++tick)
                inputCmd.Inputs.Add(SimulationSteps[tick % BufferLength].Input);

            ReceivedServerInputs.Enqueue(inputCmd);
        }

        ++ClientTick;

        if (ReceivedClientSnapshots.Count > 0 && Time.time >= ReceivedClientSnapshots.Peek().DeliveryTime)
        {
            Snapshot snapshot = ReceivedClientSnapshots.Dequeue();

            while (ReceivedClientSnapshots.Count > 0 && Time.time >= ReceivedClientSnapshots.Peek().DeliveryTime)
                snapshot = ReceivedClientSnapshots.Dequeue();

            ClientLastAckedTick = snapshot.Tick;

            ClientRb.position = snapshot.Position;
            ClientRb.rotation = snapshot.Rotation;
            ClientRb.velocity = snapshot.Velocity;
            ClientRb.angularVelocity = snapshot.AngularVelocity;

            Debug.Log("REWIND " + snapshot.Tick + " (rewinding " + (ClientTick - snapshot.Tick) + " ticks)");

            int TicksToRewind = snapshot.Tick;

            while (TicksToRewind < ClientTick)
            {
                int rewindTick = TicksToRewind % BufferLength;
                SetStateAndRollback(ref SimulationSteps[rewindTick], ClientRb);
                ++TicksToRewind;
            }
        }
    }

    void MoveLocalEntity(Rigidbody rb, Inputs input)
    {
        Vector3 direction = default;

        if (input.IsDown(BTN_FORWARD)) direction += transform.forward;
        if (input.IsDown(BTN_BACKWARD)) direction -= transform.forward;
        if (input.IsDown(BTN_LEFTWARD)) direction -= transform.right;
        if (input.IsDown(BTN_RIGHTWARD)) direction += transform.right;

        rb.velocity += direction.normalized * 3f;
    }

    void SetStateAndRollback(ref SimulationStep state, Rigidbody _rb)
    {
        state.Position = _rb.position;
        state.Rotation = _rb.rotation;

        MoveLocalEntity(_rb, state.Input);
        ClientPhysicsScene.Simulate(Time.fixedDeltaTime);
    }
}

