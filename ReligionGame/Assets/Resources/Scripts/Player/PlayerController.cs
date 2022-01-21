using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform camTransform;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ClientSend.PlayerShoot(camTransform.forward);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            ClientSend.PlayerThrowItem(camTransform.forward);
        }
    }

    private void FixedUpdate()
    {
        if (!ChatManager.isPlayerTypingMessage)
        {
            SendInputToServer();
        }
    }

    /// <summary>Sends player input to the server.</summary>
    private void SendInputToServer()
    {
        Dictionary<string, bool> _inputsAnimation = new Dictionary<string, bool>()
        {
            { "Forward", Input.GetKey(KeyCode.W) },
            { "Backward", Input.GetKey(KeyCode.S) },
            { "Left", Input.GetKey(KeyCode.A) },
            { "Right", Input.GetKey(KeyCode.D) },
            { "Jump", Input.GetKey(KeyCode.Space) }
        };

        GameManager.players[Client.instance.myId].SetPlayerStateAnimation(_inputsAnimation);

        ClientSend.PlayerMovement(_inputsAnimation);
    }
}
