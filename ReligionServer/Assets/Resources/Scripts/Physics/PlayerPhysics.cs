using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPhysics : MonoBehaviour
{
    [HideInInspector]
    public PhysicsProcess networkPhycics;

    public Transform groundCheck;
    public Rigidbody rb;

    public bool isGrounded;
    public bool isSprint;

    public uint client_tick;
    public RaycastHit slopeHit;


    private void Start()
    {
        networkPhycics = NetworkManager.instance.PhycicsProcess.GetComponent<PhysicsProcess>();

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    public void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, networkPhycics.groundDistance, networkPhycics.groundMask);
        // падение
        ControlDrag();
    }

    public void PrePhysicsStep(Commands inputs)
    {
        ControlSpeed(inputs);
        // if player is active
        MoveHZCommand gog = new MoveHZCommand();
        gog.execute(this, inputs);

        if (inputs.jump)
        {
            JumpCommand go = new JumpCommand();
            go.execute(this, inputs);
        }
    }

    public void ControlDrag()
    {
        if (networkPhycics.isDragSystem)
        {
            if (isGrounded)
            {
                rb.drag = networkPhycics.groundDrag;
            }
            else
            {
                rb.drag = networkPhycics.airDrag;
            }
        }
    }

    public void ControlSpeed(Commands input)
    {
        if (input.sprint && isGrounded)
        {
            isSprint = true;
        }
        else
        {
            isSprint = false;
        }
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, networkPhycics.playerHeight / 2 + 0.5f))
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
        GUI.Box(new Rect(5f, 120f, 120f, 25f), $"player_tick {client_tick}");
    }
}

