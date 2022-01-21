using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TouchableStructure : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Touched");
        if(other.tag == "Player")
        {
            int playerId = other.GetComponent<Player>().id;
            uint currentSpawnedObjectId = GetComponent<SpawnedGameObject>().spawnedObjectId;
            ServerSend.PlayerCanUseTool(playerId, currentSpawnedObjectId);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("UnTouched");
        if (other.tag == "Player")
        {
            int playerId = other.GetComponent<Player>().id;
            uint currentSpawnedObjectId = GetComponent<SpawnedGameObject>().spawnedObjectId;
            ServerSend.PlayerRemoveUseTool(playerId, currentSpawnedObjectId);
        }
    }
}

