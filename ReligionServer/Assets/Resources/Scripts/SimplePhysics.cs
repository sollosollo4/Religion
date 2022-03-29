using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SimplePhysics : MonoBehaviour
{

    #region TODO

    //Fix rotation jitter
    //Add remote interpolation handling
    //Rewrite snapshot structure for remote players

    #endregion

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

    public void SetSimObject(GameObject _client)
    {
        ServerSimObject = _client;
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

    [SerializeField] GameObject ServerSimObject;

    [SerializeField] int ServerTick;

    Queue<Snapshot> ReceivedClientSnapshots;

    Queue<InputCmd> ReceivedServerInputs;

    SimulationStep[] SimulationSteps;

    LoadSceneParameters sceneParams = new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.Physics3D);

    Scene ServerScene;
    PhysicsScene ServerPhysics;

    InputCmd inputCmd;

    Rigidbody ServerRb;
    Rigidbody ClientRb;

    [SerializeField] float RotationSpeed = 90;
    float CamRotation;

    float FixedStepAccumulator;

    Vector3 PreviousPosition;

    void Start()
    {
        Physics.autoSimulation = false;

        ReceivedServerInputs = new Queue<InputCmd>();
        ReceivedClientSnapshots = new Queue<Snapshot>();

        SimulationSteps = new SimulationStep[BufferLength];

        ServerScene = SceneManager.LoadScene("Game", sceneParams);
        ServerPhysics = ServerScene.GetPhysicsScene();
        SceneManager.MoveGameObjectToScene(ServerSimObject, ServerScene);

        ServerRb = ServerSimObject.GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        FixedStepAccumulator += Time.deltaTime;

        while (FixedStepAccumulator >= Time.fixedDeltaTime)
        {
            FixedStepAccumulator -= Time.fixedDeltaTime;

            ServerUpdate();
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

    void ServerUpdate()
    {
        while (ReceivedServerInputs.Count > 0 && Time.time >= ReceivedServerInputs.Peek().DeliveryTime)
        {
            InputCmd inputCmd = ReceivedServerInputs.Dequeue();

            if ((inputCmd.LastAckedTick + inputCmd.Inputs.Count - 1) >= ServerTick)
            {
                for (int i = (ServerTick > inputCmd.LastAckedTick ? (ServerTick - inputCmd.LastAckedTick) : 0); i < inputCmd.Inputs.Count; ++i)
                {
                    MoveLocalEntity(ServerRb, inputCmd.Inputs[i]);
                    ServerPhysics.Simulate(Time.fixedDeltaTime);

                    ++ServerTick;

                    if (Random.value > PACKET_LOSS)
                    {
                        Snapshot snapshot;
                        snapshot.DeliveryTime = Time.time + RTT;
                        snapshot.Tick = ServerTick;
                        snapshot.Position = ServerRb.position;
                        snapshot.Rotation = ServerRb.rotation;
                        snapshot.Velocity = ServerRb.velocity;
                        snapshot.AngularVelocity = ServerRb.angularVelocity;

                        ReceivedClientSnapshots.Enqueue(snapshot);
                    }
                }
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
}

