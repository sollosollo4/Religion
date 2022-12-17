using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform camTransform;

    private void Start()
    {
        camTransform = GameManager.instance.moveCamera.transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ClientSend.PlayerShoot(camTransform.forward * 4f);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            ClientSend.PlayerThrowItem(camTransform.forward * 4f);
        }
    }

    private void FixedUpdate()
    {
        if (!ChatManager.isPlayerTypingMessage && !GameManager.players[Client.instance.myId].IsTool)
        {
            SendInputToServer();
        }
    }

    /// <summary>Sends player input to the server.</summary>
    private void SendInputToServer()
    {
        /*
            Dictionary<string, bool> _inputsMovementsAnimation = new Dictionary<string, bool>()
            {
                { "Forward", Input.GetKey(KeyCode.W) },
                { "Backward", Input.GetKey(KeyCode.S) },
                { "Left", Input.GetKey(KeyCode.A) },
                { "Right", Input.GetKey(KeyCode.D) },
                { "Jump", Input.GetKey(KeyCode.Space) }
            };

            GameManager.players[Client.instance.myId].SetPlayerMovementStateAnimation(_inputsMovementsAnimation);

            Dictionary<string, bool> _inputsWorkAnimation = new Dictionary<string, bool>()
            {
                { "UseTool", Input.GetKey(KeyCode.E) },
            };

            GameManager.players[Client.instance.myId].SetWork(_inputsWorkAnimation);

            ClientSend.PlayerMovement(_inputsMovementsAnimation);
            ClientSend.PlayerAnimation(GameManager.players[Client.instance.myId].animationState);
        */
    }
}
