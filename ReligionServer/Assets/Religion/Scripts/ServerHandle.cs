using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandle
{
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();

        Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
        if (_fromClient != _clientIdCheck)
        {
            Debug.Log($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
        }
        Server.clients[_fromClient].SendIntoGame(_username);
    }

    public static void PlayerMovement(int _fromClient, Packet _packet)
    {
        bool[] _inputs = new bool[_packet.ReadInt()];
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }
        Quaternion _rotation = _packet.ReadQuaternion();
        int animationState = _packet.ReadInt();

        Server.clients[_fromClient].player.SetInput(_inputs, _rotation, animationState);
        ServerSend.PlayerAnimationState(Server.clients[_fromClient].player);
    }

    public static void PlayerShoot(int _fromClient, Packet _packet)
    {
        Vector3 _shootDirection = _packet.ReadVector3();

        Server.clients[_fromClient].player.Shoot(_shootDirection);
    }

    public static void PlayerThrowItem(int _fromClient, Packet _packet)
    {
        Vector3 _throwDirection = _packet.ReadVector3();

        Server.clients[_fromClient].player.ThrowItem(_throwDirection);
    }

    public static void PlayerChatMessage(int _fromClient, Packet _packet)
    {
        string _message = _packet.ReadString();
        ChatManager.MessageController(_fromClient, _message);
    }

    public static void PlayerTryConnection(int _fromClient, Packet _packet)
    {
        string _login = _packet.ReadString();
        string _password = _packet.ReadString();

        if (AuthorizationAtDatabase.CheckUserPasswordhash(_login, _password, _fromClient))
        {
            Server.clients[_fromClient].getCharacters();
            Server.clients[_fromClient].AuthConnection(_fromClient, true);
        }
        else
        {
            Server.clients[_fromClient].AuthConnection(_fromClient, false);
        }
    }

    public static void CharacterNew(int _fromClient, Packet _packet)
    {
        string _name = _packet.ReadString();
        string _className = _packet.ReadString();
        Character newChar = new Character()
        {
            CharacterName = _name,
            CharacterClass = CharacterClass.CreateClassByName(_className),
            AccountId = Server.clients[_fromClient].accountId
        };

        bool isCreated = Character.CreateNewCharacter(newChar);
        ServerSend.CreateNewCharacter(_fromClient, newChar, isCreated);
    }

    public static void AnimationState(int _fromClient, Packet _packet)
    {
        Server.clients[_fromClient].player.animationState = _packet.ReadInt();
    }
}
