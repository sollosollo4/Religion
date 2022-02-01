using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    /// <summary>Sends a packet to the server via TCP.</summary>
    /// <param name="_packet">The packet to send to the sever.</param>
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    /// <summary>Sends a packet to the server via UDP.</summary>
    /// <param name="_packet">The packet to send to the sever.</param>
    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    /// <summary>Lets the server know that the welcome message was received.</summary>
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write("TestPlayer");

            SendTCPData(_packet);
        }
    }

    /// <summary>Sends player input to the server.</summary>
    /// <param name="_inputs"></param>
    public static void PlayerMovement(Dictionary<string, bool> _inputs)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_inputs.Count);
            foreach (var _input in _inputs)
            {
                _packet.Write(_input.Value);
            }
            _packet.Write(GameManager.players[Client.instance.myId].transform.rotation);

            SendUDPData(_packet);
        }
    }

    public static void PlayerShoot(Vector3 _facing)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerShoot))
        {
            _packet.Write(_facing);

            SendTCPData(_packet);
        }
    }

    public static void PlayerThrowItem(Vector3 _facing)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerThrowItem))
        {
            _packet.Write(_facing);

            SendTCPData(_packet);
        }
    }

    public static void PlayerSendChatMessage(string _message)
    {
        using (Packet _packet = new Packet((int)ClientPackets.chatMessage))
        {
            _packet.Write(_message);

            SendTCPData(_packet);
        }
    }
    
    public static void PlayerAnimation(int animationState)
    {
        using (Packet _packet = new Packet((int)ClientPackets.animationState))
        {
            _packet.Write(GameManager.players[Client.instance.myId].animationState);

            SendUDPData(_packet);
        }
    }

    public static void CreateNewCharater(string _characterName, string _className)
    {
        using (Packet _packet = new Packet((int)ClientPackets.createNewCharacter))
        {
            _packet.Write(_characterName);
            _packet.Write(_className);

            SendTCPData(_packet);
        }
    }

    public static void PlayerUseTool(uint _spawnableObjectid, byte animationState)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerUseTool))
        {
            _packet.Write(_spawnableObjectid);
            _packet.Write(animationState);

            SendTCPData(_packet);
        }
    }
    #endregion
}
