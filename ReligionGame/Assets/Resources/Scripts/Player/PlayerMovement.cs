using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float playerHeight = 1.9f;
    public bool isSprint;
    [Header("Movement")]
    [SerializeField] public float moveSpeed = 6f;
    [SerializeField] public float airMultiplier = 0.4f;

    [Header("Sprinting")]
    [SerializeField] public float walkSpeed = 4f;
    [SerializeField] public float sprintSpeed = 6f;
    [SerializeField] public float movementMultiplier = 10f;

    [Header("Drag")]
    [SerializeField] public float groundDrag = 6f;
    [SerializeField] public float airDrag = 2f;

    [Header("Jumping")]
    public float jumpForce = 5f;
    public float jumpCooldown = 1.5f;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

    
    [Header("Ground Detection")]
    public Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.2f;
    
   

    public Rigidbody rb;
    public float horizontalMovement;
    public float verticalMovement;
    public RaycastHit slopeHit;
    public bool isGrounded;

    private float client_timer;
    private uint client_tick_number;
    private uint client_last_received_state_tick;
    private const int c_client_buffer_size = 32768;
    private ClientState[] client_state_buffer; // здесь клиент хранит предсказанные ходы
    private Queue<StateMessage> client_state_msgs; // клиент хранит прогнозируемые входные данные здесь

    private Commands[] client_command_buffer; // клиент хранит прогнозируемые входные данные здесь


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Physics.IgnoreLayerCollision(8, 8); // ignore players <-> players collisions touchs

        client_timer = 0.0f;
        client_tick_number = 0;
        client_last_received_state_tick = 0;
        client_state_buffer = new ClientState[c_client_buffer_size];
        client_command_buffer = new Commands[c_client_buffer_size];
        client_state_msgs = new Queue<StateMessage>();
    }

    private void Update()
    {
        // проверяем на почве ли мы
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        // падение
        ControlDrag(); // just a control
        // ускоряемся ли?
        ControlSpeed(); // just a speed control
    }

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        float client_timer = this.client_timer;
        uint client_tick_number = this.client_tick_number;

        client_timer += Time.deltaTime;
        while (client_timer >= dt)
        {
            client_timer -= dt;
            uint buffer_slot = client_tick_number % c_client_buffer_size;

            // рассчитываем движение
            Commands moveCommand = MyInput();

            client_command_buffer[buffer_slot] = moveCommand;

            // отправить входной пакет на сервер
            CommandMessage input_msg;
            input_msg.start_tick_number = client_last_received_state_tick;
            input_msg.inputs = new List<Commands>();

            for (uint tick = input_msg.start_tick_number; tick <= client_tick_number; ++tick)
            {
                input_msg.inputs.Add(client_command_buffer[tick % c_client_buffer_size]);
            }

            // сохранить состояние для этого тика, затем использовать текущее состояние + ввод для пошаговой симуляции
            ClientStoreCurrentStateAndStep(ref client_state_buffer[buffer_slot], rb, moveCommand, dt);

            ClientSend.PlayerMovement(input_msg);

            ++client_tick_number;
        }
        this.client_timer = client_timer;
        this.client_tick_number = client_tick_number;

        if (ClientHasStateMessage())
        {
            StateMessage state_msg = client_state_msgs.Dequeue();
            while (ClientHasStateMessage()) // убедитесь, что доступны какие-либо более новые сообщения о состоянии, вместо этого мы используем их
            {
                state_msg = client_state_msgs.Dequeue();
            }

            client_last_received_state_tick = state_msg.tick_number;

            uint buffer_slot = state_msg.tick_number % c_client_buffer_size;
            Vector3 position_error = state_msg.position - client_state_buffer[buffer_slot].position;
            
            if (position_error.sqrMagnitude > 0.0000001f)
            {
                //Debug.Log("Correcting for error at tick " + state_msg.tick_number + " (rewinding " + (client_tick_number - state_msg.tick_number) + " ticks)");

                // перемотать и воспроизвести
                rb.position = state_msg.position;

                uint rewind_tick_number = state_msg.tick_number;
                while (rewind_tick_number < client_tick_number)
                {
                    buffer_slot = rewind_tick_number % c_client_buffer_size;

                    ClientStateStep(client_command_buffer[buffer_slot], dt);

                    ++rewind_tick_number;
                }
            }
        }

        transform.position = rb.position;
    }

    public bool isDragSystem;

    Commands MyInput()
    {
        Commands command = new Commands();
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
        
        command.moveHorizontal = Convert.ToSByte(horizontalMovement);
        command.moveVertical = Convert.ToSByte(verticalMovement);
        command.sprint = Input.GetKey(sprintKey);
        
        if (Input.GetKey(jumpKey) && isGrounded)
        {
            command.jump = true;
        }

        command.orientation = Convert.ToInt16(GetComponent<PlayerLook>().orientation.eulerAngles.y);

        return command;
    }

    void ControlSpeed()
    {
        if (Input.GetKey(sprintKey) && isGrounded)
        {
            isSprint = true;
        }
        else
        {
            isSprint = false;
        }
        GetComponent<PlayerManager>().IsSprint = isSprint;
    }

    public void ControlDrag()
    {
        if (isDragSystem)
        {
            if (isGrounded)
            {
                rb.drag = groundDrag;
            }
            else
            {
                rb.drag = airDrag;
            }
        }
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    void MovePlayer(Commands inputs)
    {
        MoveHZCommand gog = new MoveHZCommand();
        gog.execute(this, inputs);
        

        if(inputs.jump) 
        {
            JumpCommand go = new JumpCommand();
            go.execute(this, inputs);
        }
        
    }

    public void SetStateMessages(StateMessage _stateMessage)
    {
        client_state_msgs.Enqueue(_stateMessage);
    }

    private bool ClientHasStateMessage()
    {
        return client_state_msgs.Count > 0;
    }

    private void ClientStoreCurrentStateAndStep(ref ClientState current_state, Rigidbody rigidbody, Commands inputs, float dt)
    {
        current_state.position = rigidbody.position;

        MovePlayer(inputs);
        GameManager.instance.MainScene.GetPhysicsScene().Simulate(dt);
    }

    private void ClientStateStep(Commands inputs, float dt)
    {
        MovePlayer(inputs);
        GameManager.instance.MainScene.GetPhysicsScene().Simulate(dt);
    }
}
