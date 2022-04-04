using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPhysics : MonoBehaviour
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

    [Header("Ground Detection")]
    public Transform groundCheck;
    [SerializeField] public LayerMask groundMask;
    [SerializeField] public float groundDistance = 0.2f;
    public bool isGrounded;

    public Rigidbody rb;
    RaycastHit slopeHit;

    // server specific
    private uint server_tick_number;
    public Queue<InputMessage> server_input_msgs;
    private Scene server_scene;
    private PhysicsScene server_physics_scene;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        server_tick_number = 0;
        server_input_msgs = new Queue<InputMessage>();

        server_scene = SceneManager.GetActiveScene();
        server_physics_scene = server_scene.GetPhysicsScene();
    }

    public void SetInput(InputMessage _inputMessage)
    {
        server_input_msgs.Enqueue(_inputMessage);
    }
       
    private bool ServerHasInputMessage()
    {
         return server_input_msgs.Count > 0 && Time.time >= server_input_msgs.Peek().delivery_time;
    }

    public void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        // падение
        ControlDrag();
        uint server_tick_number = this.server_tick_number;
        float dt = Time.fixedDeltaTime;
        while (ServerHasInputMessage())
        {
            InputMessage input_msg = server_input_msgs.Dequeue();

            // содержит массив входных данных, вычислите, какой тик является последним
            uint max_tick = input_msg.start_tick_number + (uint)input_msg.inputs.Count - 1;

            // если этот тик больше или равен текущему тику, на котором мы находимся, то он
            // имеет новые входные данные
            //Debug.Log("Ticks: " + max_tick + " - " + server_tick_number + ". Count:" + server_input_msgs.Count);

            if (max_tick >= server_tick_number)
            {
                // в массиве могут быть какие-то входы, которые у нас уже были,
                // так что разберитесь с чего начать
                uint start_i = server_tick_number > input_msg.start_tick_number ? (server_tick_number - input_msg.start_tick_number) : 0;

                // просмотреть все соответствующие входные данные и сделать шаг вперед игрока
                for (int i = (int)start_i; i < input_msg.inputs.Count; ++i)
                {
                    // ускоряемся ли?
                    ControlSpeed(input_msg.inputs[i]);
                    PrePhysicsStep(input_msg.inputs[i]);
                    server_physics_scene.Simulate(dt);

                    ++server_tick_number;

                    StateMessage state_msg;
                    state_msg.delivery_time = Time.time;
                    state_msg.tick_number = server_tick_number;
                    state_msg.position = rb.position;
                    ServerSend.PlayerPosition(GetComponent<Player>().id, state_msg);

                    transform.position = rb.position;
                }
            }
        }      
        this.server_tick_number = server_tick_number;
    }

    public void PrePhysicsStep(Inputs inputs)
    {
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(inputs.moveD.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(inputs.slopeD.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(inputs.moveD.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && inputs.jump)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Acceleration);
        }
    }

    public void ControlDrag()
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

    public void ControlSpeed(Inputs input)
    {
        if (input.sprint && isGrounded)
        {
            moveSpeed = sprintSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }
    }

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

    private void OnGUI()
    {
        GUI.Box(new Rect(5f, 35f, 180f, 25f), $"server_tick_number {server_tick_number}");
    }
}

