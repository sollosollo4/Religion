using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TouchableStructure : MonoBehaviour
{
    public float MiningTime = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            int playerId = other.GetComponent<Player>().id;
            uint currentSpawnedObjectId = GetComponent<SpawnedGameObject>().spawnedObjectId;
            ServerSend.PlayerTouchStructure(playerId, currentSpawnedObjectId, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            int playerId = other.GetComponent<Player>().id;
            uint currentSpawnedObjectId = GetComponent<SpawnedGameObject>().spawnedObjectId;
            ServerSend.PlayerTouchStructure(playerId, currentSpawnedObjectId, false);
        }
    }

    public void StartMining(int _fromClient)
    {
        StartCoroutine(Mining(MiningTime, _fromClient));
    }

    private IEnumerator Mining(float miningTime, int _fromClient)
    {
        yield return new WaitForSeconds(miningTime);

        Server.clients[_fromClient].player.isTool = false;

        uint currentSpawnedObjectId = GetComponent<SpawnedGameObject>().spawnedObjectId;
        ServerSend.PlayerEndMining(_fromClient, currentSpawnedObjectId);
    }
}

