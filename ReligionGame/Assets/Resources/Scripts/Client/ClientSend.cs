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
    public static void WelcomeReceived(int currentCharacter)
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(currentCharacter);

            SendTCPData(_packet);
        }
    }

    internal static void PlayerStuck()
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerStucks))
        {
            _packet.Write(Client.instance.myId);

            SendTCPData(_packet);
        }
    }

    public static void PlayerShoot(Vector3 _facing)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerShoot))
        {
            _packet.Write(_facing);

            SendUDPData(_packet);
        }
    }

    public static void PlayerThrowItem(Vector3 _facing)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerThrowItem))
        {
            _packet.Write(_facing);

            SendUDPData(_packet);
        }
    }

    public static void PlayerSendChatMessage(string _message)
    {
        using (Packet _packet = new Packet((int)ClientPackets.chatMessage))
        {
            _packet.Write(_message);

            SendUDPData(_packet);
        }
    }

    public static void CreateNewCharater(string _characterName, string _className)
    {
        using (Packet _packet = new Packet((int)ClientPackets.createNewCharacter))
        {
            _packet.Write(_characterName);
            _packet.Write(_className);

            SendUDPData(_packet);
        }
    }

    public static void PlayerUseTool(uint _spawnableObjectid)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerUseTool))
        {
            _packet.Write(_spawnableObjectid);

            SendUDPData(_packet);
        }
    }

    public static void PlayerMovement(InputMessage _inputMessage)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_inputMessage.inputs.Count);
            foreach (Inputs _input in _inputMessage.inputs)
            {
                _packet.Write(_input.moveD);
                _packet.Write(_input.slopeD);
                _packet.Write(_input.jump);
                _packet.Write(_input.sprint);
            }
            _packet.Write(_inputMessage.camRotation);
            _packet.Write(_inputMessage.delivery_time);
            _packet.Write(_inputMessage.start_tick_number);

            SendUDPData(_packet);
        }
    }

    public static void PlayerMovement(CommandMessage _commandMessage)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_commandMessage.inputs.Count);
            _packet.Write(_commandMessage.start_tick_number);

            foreach (Commands _input in _commandMessage.inputs)
            {
                _packet.Write(_input.jump);
                _packet.Write(_input.sprint);
                _packet.Write(_input.moveHorizontal);
                _packet.Write(_input.moveVertical);
                _packet.Write(_input.orientation);
            }

            SendUDPData(_packet);
        }
    }
    #endregion
}
