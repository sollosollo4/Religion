using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhysicsProcess : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] public float playerHeight = 1.9f;

    [Header("Movement")]
    [SerializeField] public float moveSpeed = 6f;
    [SerializeField] public float airMultiplier = 0.4f;

    [Header("Sprinting")]
    [SerializeField] public float walkSpeed = 4f;
    [SerializeField] public float sprintSpeed = 6f;
    [SerializeField] public float acceleration = 10f;

    [Header("Drag")]
    [SerializeField] public float groundDrag = 6f;
    [SerializeField] public float airDrag = 2f;

    [Header("Jumping")]
    [SerializeField] public float jumpForce = 5f;

    [Header("Ground Detection")]
    [SerializeField] public LayerMask groundMask;
    [SerializeField] public float groundDistance = 0.2f;

    // server specific
    private Dictionary<int, uint> server_tick_number;
    // Client <-> inputs 
    public Dictionary<int, Queue<CommandMessage>> server_input_msgs;
    private Scene server_scene;
    private PhysicsScene server_physics_scene;

    public bool isDragSystem;

    private void Start()
    {
        Physics.IgnoreLayerCollision(8, 8); // ignore players <-> players collisions touchs

        server_tick_number = new Dictionary<int, uint>();
        server_input_msgs = new Dictionary<int, Queue<CommandMessage>>();

        server_scene = SceneManager.GetActiveScene();
        server_physics_scene = server_scene.GetPhysicsScene();
    }

    public void SetMovementMessage(int client, CommandMessage commandMsg)
    {
        // Old client
        if(server_input_msgs.TryGetValue(client, out Queue<CommandMessage> msg))
        {
            msg.Enqueue(commandMsg);
        }
        else
        {
            // New client
            Queue<CommandMessage> cmdsMsgs = new Queue<CommandMessage>();
            cmdsMsgs.Enqueue(commandMsg);
            server_input_msgs.Add(client, cmdsMsgs);
            server_tick_number.Add(client, 0);
        }
        
    }

    private bool ServerHasInputMessage(Queue<CommandMessage> msgs)
    {
        return msgs.Count > 0;
    }

    public void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        foreach (var ClientInputMessages in server_input_msgs)
        {
            uint server_tick_number = this.server_tick_number[ClientInputMessages.Key];
            PlayerPhysics playerP = Server.clients[ClientInputMessages.Key].player.GetComponent<PlayerPhysics>();
            while (ServerHasInputMessage(ClientInputMessages.Value))
            {
                CommandMessage input_msg = ClientInputMessages.Value.Dequeue();

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
                        playerP.PrePhysicsStep(input_msg.inputs[i]);
                        server_physics_scene.Simulate(dt);

                        ++server_tick_number;

                        StateMessage state_msg;
                        state_msg.delivery_time = Time.time;
                        state_msg.tick_number = server_tick_number;
                        state_msg.position = playerP.rb.position;

                        ServerSend.PlayerPosition(ClientInputMessages.Key, state_msg);

                        playerP.transform.position = playerP.rb.position;
                    }
                }
            }
            this.server_tick_number[ClientInputMessages.Key] = server_tick_number;
        }
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(5f, 35f, 180f, 25f), $"server_tick_number {server_tick_number}");
    }
}

