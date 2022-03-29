using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        int _inputsCount = _packet.ReadInt();
        InputMessage inputMsg;
        inputMsg.inputs = new List<Inputs>(_inputsCount);
        for (int i = 0; i < _inputsCount; i++)
        {
            Inputs inpt;
            inpt.moveD = _packet.ReadVector3();
            inpt.slopeD = _packet.ReadVector3();
            inpt.jump = _packet.ReadBool();
            inpt.sprint = _packet.ReadBool();
            inputMsg.inputs.Add(inpt);
        }
        inputMsg.camRotation = _packet.ReadFloat();
        inputMsg.delivery_time = _packet.ReadFloat();
        inputMsg.start_tick_number = _packet.ReadUint();

        Server.clients[_fromClient].player.GetComponent<PlayerPhysics>().SetInput(inputMsg);
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

        Debug.Log(_login + " <--> " + _password);

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
        Server.clients[_fromClient].player.animationState = _packet.ReadByte();
    }

    public static void PlayerUseTool(int _fromClient, Packet _packet)
    {
        uint _structureId = _packet.ReadUint();
        byte _animationId = _packet.ReadByte();

        SpawnedGameObject touchableElement = UnityEngine.Object.FindObjectsOfType<SpawnedGameObject>().First(el => el.spawnedObjectId == _structureId);

        touchableElement.GetComponent<TouchableStructure>().StartMining(_fromClient);

        Server.clients[_fromClient].player.isTool = true;
        Server.clients[_fromClient].player.animationState = _animationId;
    }
}
