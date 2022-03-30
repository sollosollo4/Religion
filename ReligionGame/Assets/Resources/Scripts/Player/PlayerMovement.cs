using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    float playerHeight = 1.7f;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float airMultiplier = 0.4f;
    float movementMultiplier = 10f;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;

    [Header("Jumping")]
    public float jumpForce = 5f;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;   

    float horizontalMovement;
    float verticalMovement;

    [Header("Ground Detection")]
    public Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.2f;
    bool isGrounded;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    Rigidbody rb;

    RaycastHit slopeHit;

    private float client_timer;
    private uint client_tick_number;
    private uint client_last_received_state_tick;
    private const int c_client_buffer_size = 2048;
    private ClientState[] client_state_buffer; // здесь клиент хранит предсказанные ходы
    private Inputs[] client_input_buffer; // клиент хранит прогнозируемые входные данные здесь
    private Queue<StateMessage> client_state_msgs;
    private Vector3 client_pos_error;
    private bool isJump;

    private bool OnSlope()
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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        client_timer = 0.0f;
        client_tick_number = 0;
        client_last_received_state_tick = 0;
        client_state_buffer = new ClientState[c_client_buffer_size];
        client_input_buffer = new Inputs[c_client_buffer_size];
        client_state_msgs = new Queue<StateMessage>();
        client_pos_error = Vector3.zero;
    }

    private void Update()
    {
        // проверяем на почве ли мы
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // рассчитываем движение
        MyInput();
        // падение
        ControlDrag();
        // ускоряемся ли?
        ControlSpeed();
        // обрабатываем прыжок
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            isJump = true;
        }

        // рассчитываем склоны
        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

        float dt = Time.fixedDeltaTime;
        float client_timer = this.client_timer;
        uint client_tick_number = this.client_tick_number;

        client_timer += Time.deltaTime;
        while (client_timer >= dt)
        {
            client_timer -= dt;

            uint buffer_slot = client_tick_number % c_client_buffer_size;

            // выборка и сохранение входных данных для этого тика
            Inputs inputs;
            inputs.moveD = moveDirection;
            inputs.slopeD = slopeMoveDirection;
            inputs.jump = isJump;
            inputs.sprint = Input.GetKey(sprintKey);
            client_input_buffer[buffer_slot] = inputs;

            // отправить входной пакет на сервер
            InputMessage input_msg;
            input_msg.delivery_time = Time.time;
            input_msg.start_tick_number = client_last_received_state_tick;
            input_msg.inputs = new List<Inputs>();
            input_msg.camRotation = GetComponent<PlayerLook>().orientation.rotation.y;

            for (uint tick = input_msg.start_tick_number; tick <= client_tick_number; ++tick)
            {
                input_msg.inputs.Add(client_input_buffer[tick % c_client_buffer_size]);
            }

            // сохранить состояние для этого тика, затем использовать текущее состояние + ввод для пошаговой симуляции
            ClientStoreCurrentStateAndStep(
                ref client_state_buffer[buffer_slot],
                rb,
                inputs,
                dt);

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

                // захватить текущую прогнозируемую позицию для сглаживания
                //Vector3 prev_pos = rb.position + client_pos_error;

                // перемотать и воспроизвести
                rb.position = state_msg.position;
                //rb.velocity = state_msg.velocity;
                //rb.angularVelocity = state_msg.angular_velocity;

                uint rewind_tick_number = state_msg.tick_number;
                while (rewind_tick_number < client_tick_number)
                {
                    buffer_slot = rewind_tick_number % c_client_buffer_size;
                    ClientStoreCurrentStateAndStep(
                        ref client_state_buffer[buffer_slot],
                        rb,
                        client_input_buffer[buffer_slot],
                        dt);

                    ++rewind_tick_number;
                }

                // если разница больше 2 мс, просто щелкнуть
                /*if ((prev_pos - rb.position).sqrMagnitude >= 4.0f)
                {
                    client_pos_error = Vector3.zero;
                }
                else
                {
                    client_pos_error = prev_pos - rb.position;
                }*/
            }
        }

        //client_pos_error *= 0.9f;

        transform.position = Vector3.Lerp(transform.position, rb.position, 0.2f);

        if (Time.unscaledTime > _timer)
        {
            fps = (int)(1f / Time.deltaTime);
            _timer = Time.unscaledTime + 1;
        }
    }

    int fps;
    float _timer;

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        GetComponentInChildren<Animator>().SetFloat("horizontal", horizontalMovement);
        GetComponentInChildren<Animator>().SetFloat("vertical", verticalMovement);

        moveDirection = GetComponent<PlayerLook>().orientation.forward * verticalMovement + GetComponent<PlayerLook>().orientation.right * horizontalMovement;
    }

    void ControlSpeed()
    {
        if (Input.GetKey(sprintKey) && isGrounded)
        {
            moveSpeed = sprintSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }
    }

    void ControlDrag()
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

    void MovePlayer(Inputs inputs)
    {
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
        else if(isGrounded && inputs.jump)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Acceleration);
        }
    }

    public void SetStateMessages(StateMessage _stateMessage)
    {
        client_state_msgs.Enqueue(_stateMessage);
    }

    private bool ClientHasStateMessage()
    {
        return client_state_msgs.Count > 0; /* && Time.time >= client_state_msgs.Peek().delivery_time*/
    }

    private void ClientStoreCurrentStateAndStep(ref ClientState current_state, Rigidbody rigidbody, Inputs inputs, float dt)
    {
        current_state.position = rigidbody.position;

        MovePlayer(inputs);
        GameManager.instance.MainScene.GetPhysicsScene().Simulate(dt);
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(5f, 35f, 180f, 25f), $"State messages {client_state_msgs.Count}");
        GUI.Box(new Rect(5f, 95f, 180f, 25f), $"Client timer {client_timer}");
        GUI.Box(new Rect(5f, 125f, 180f, 25f), $"LAST TICK {client_tick_number}");
        GUI.Box(new Rect(5f, 155f, 180f, 25f), $"SERVER TICK {client_last_received_state_tick}");
        GUI.Box(new Rect(5f, 185f, 180f, 25f), $"FPS {fps}");
    }
}
