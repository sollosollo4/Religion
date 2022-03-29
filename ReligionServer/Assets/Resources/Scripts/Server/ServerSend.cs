using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend
{
    /// <summary>Sends a packet to a client via TCP.</summary>
    /// <param name="_toClient">The client to send the packet the packet to.</param>
    /// <param name="_packet">The packet to send to the client.</param>
    protected static void SendTCPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].tcp.SendData(_packet);
    }

    /// <summary>Sends a packet to a client via UDP.</summary>
    /// <param name="_toClient">The client to send the packet the packet to.</param>
    /// <param name="_packet">The packet to send to the client.</param>
    protected static void SendUDPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].udp.SendData(_packet);
    }

    /// <summary>Sends a packet to all clients via TCP.</summary>
    /// <param name="_packet">The packet to send.</param>
    protected static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(_packet);
        }
    }

    /// <summary>Sends a packet to all clients except one via TCP.</summary>
    /// <param name="_exceptClient">The client to NOT send the data to.</param>
    /// <param name="_packet">The packet to send.</param>
    protected static void SendTCPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
    }

    /// <summary>Sends a packet to all clients via UDP.</summary>
    /// <param name="_packet">The packet to send.</param>
    protected static void SendUDPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].udp.SendData(_packet);
        }
    }
    /// <summary>Sends a packet to all clients except one via UDP.</summary>
    /// <param name="_exceptClient">The client to NOT send the data to.</param>
    /// <param name="_packet">The packet to send.</param>
    protected static void SendUDPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
    }

    #region Packets

    /// <summary>Tells a client to spawn a player.</summary>
    /// <param name="_toClient">The client that should spawn the player.</param>
    /// <param name="_player">The player to spawn.</param>
    public static void SpawnPlayer(int _toClient, Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.username);
            _packet.Write(_player.transform.position);
            _packet.Write(_player.transform.rotation);

            SendUDPData(_toClient, _packet);
        }
    }

    /// <summary>Sends a player's updated position to all clients.</summary>
    /// <param name="_player">The player whose position to update.</param>
    public static void PlayerPosition(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerPosition))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.position);
            _packet.Write(_player.isTool);

            SendUDPDataToAll(_packet);
        }
    }

    public static void PlayerPosition(int _id, StateMessage _message)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerPosition))
        {
            _packet.Write(_id);
            _packet.Write(_message.position);
            _packet.Write(_message.tick_number);
            _packet.Write(_message.delivery_time);
            

            SendUDPDataToAll(_packet);
        }
    }

    /// <summary>Sends a player's updated rotation to all clients except to himself (to avoid overwriting the local player's rotation).</summary>
    /// <param name="_player">The player whose rotation to update.</param>
    public static void PlayerRotation(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerRotation))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.rotation);

            SendUDPDataToAll(_player.id, _packet);
        }
    }

    public static void PlayerAnimationState(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerState))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.animationState);

            SendUDPDataToAll(_player.id, _packet);
        }
    }

    public static void PlayerDisconnected(int _playerId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerDisconnected))
        {
            _packet.Write(_playerId);

            SendUDPDataToAll(_packet);
        }
    }

    public static void PlayerHealth(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerHealth))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.health);

            SendUDPDataToAll(_packet);
        }
    }

    public static void PlayerRespawned(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerRespawned))
        {
            _packet.Write(_player.id);

            SendUDPDataToAll(_packet);
        }
    }

    public static void CreateItemSpawner(int _toClient, int _spawnerId, Vector3 _spawnerPosition, bool _hasItem)
    {
        using (Packet _packet = new Packet((int)ServerPackets.createItemSpawner))
        {
            _packet.Write(_spawnerId);
            _packet.Write(_spawnerPosition);
            _packet.Write(_hasItem);

            SendUDPData(_toClient, _packet);
        }
    }

    public static void ItemSpawned(int _spawnerId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.itemSpawned))
        {
            _packet.Write(_spawnerId);

            SendUDPDataToAll(_packet);
        }
    }

    public static void ItemPickedUp(int _spawnerId, int _byPlayer)
    {
        using (Packet _packet = new Packet((int)ServerPackets.itemPickedUp))
        {
            _packet.Write(_spawnerId);
            _packet.Write(_byPlayer);

            SendUDPDataToAll(_packet);
        }
    }

    public static void SpawnProjectile(Projectile _projectile, int _thrownByPlayer)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnProjectile))
        {
            _packet.Write(_projectile.id);
            _packet.Write(_projectile.transform.position);
            _packet.Write(_thrownByPlayer);

            SendUDPDataToAll(_packet);
        }
    }

    public static void ProjectilePosition(Projectile _projectile)
    {
        using (Packet _packet = new Packet((int)ServerPackets.projectilePosition))
        {
            _packet.Write(_projectile.id);
            _packet.Write(_projectile.transform.position);

            SendUDPDataToAll(_packet);
        }
    }

    public static void ProjectileExploded(Projectile _projectile)
    {
        using (Packet _packet = new Packet((int)ServerPackets.projectileExploded))
        {
            _packet.Write(_projectile.id);
            _packet.Write(_projectile.transform.position);

            SendUDPDataToAll(_packet);
        }
    }

    public static void SpawnEnemy(Enemy _enemy)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnEnemy))
        {
            SendUDPDataToAll(SpawnEnemy_Data(_enemy, _packet));
        }
    }

    public static void SpawnEnemy(int _toClient, Enemy _enemy)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnEnemy))
        {
            SendUDPData(_toClient, SpawnEnemy_Data(_enemy, _packet));
        }
    }

    private static Packet SpawnEnemy_Data(Enemy _enemy, Packet _packet)
    {
        _packet.Write(_enemy.id);
        _packet.Write(_enemy.transform.position);
        return _packet;
    }

    public static void SpawnStructure(object id, SpawnedGameObject spGameObj)
    {
        
    }

    public static void EnemyPosition(Enemy _enemy)
    {
        using (Packet _packet = new Packet((int)ServerPackets.enemyPosition))
        {
            _packet.Write(_enemy.id);
            _packet.Write(_enemy.transform.position);

            SendUDPDataToAll(_packet);
        }
    }

    public static void EnemyHealth(Enemy _enemy)
    {
        using (Packet _packet = new Packet((int)ServerPackets.enemyHealth))
        {
            _packet.Write(_enemy.id);
            _packet.Write(_enemy.health);

            SendUDPDataToAll(_packet);
        }
    }

    public static void PlayerChatMessage(int _byPlayer, string _message)
    {
        using (Packet _packet = new Packet((int)ServerPackets.chatMessage))
        {
            _packet.Write(_byPlayer);
            _packet.Write(_message);

            SendUDPDataToAll(_packet);
        }
    }

    public static void PlayerTryConnection(int _byPlayer, bool _success, string _message, List<Character> _characters)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerTryConnection))
        {
            _packet.Write(_success);
            _packet.Write(_message);
            _packet.Write(_byPlayer);

            _packet.Write(_characters.Count);
            foreach (Character _clientCharacter in _characters)
            {
                _packet.Write(_clientCharacter.CharacterName);
                _packet.Write(_clientCharacter.CharacterClass.CharacterClassCode);
            }

            SendTCPData(_byPlayer, _packet);
        }
    }

    public static void CreateNewCharacter(int _byPlayer, Character _newChar, bool _isCreated)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerCreateNewCharacter))
        {
            _packet.Write(_isCreated);
            if (!_isCreated) _packet.Write("Current name already is use. Pleae set different name.");
            _packet.Write(_newChar.CharacterName);
            _packet.Write(_newChar.CharacterClass.CharacterClassCode);

            SendUDPData(_byPlayer, _packet);
        }
    }

    public static void PlayerTouchStructure(int _byPlayer, uint _instanceOjbject, bool _touch)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerTouchStructure))
        {
            _packet.Write(_touch);
            _packet.Write(_instanceOjbject);
            SendUDPData(_byPlayer, _packet);
        }
    }

    public static void PlayerStartMining(int _byPlayer, byte _animationId, uint _instanceOjbject)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerStartMining))
        {
            _packet.Write(_byPlayer);
            _packet.Write(_animationId);
            _packet.Write(_instanceOjbject);

            SendUDPDataToAll(_packet);
        }
    }

    public static void PlayerEndMining(int _fromClient, uint _currentSpawnedObjectId /* сюда потом добавить то, что добыл и сколько*/)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerEndMining))
        {
            _packet.Write(_fromClient);
            _packet.Write(_currentSpawnedObjectId);

            SendUDPDataToAll(_packet);
        }
    }
    #endregion
}
