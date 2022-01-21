using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[SerializeField]
public class SpawnedGameObject : MonoBehaviour
{
    public uint spawnedObjectId;
    
    public ushort PrefabId;
    public ushort parentSpawnedObjectId;
    public string ParentPath;
    public string type;   

    private Dictionary<uint, SpawnedGameObject> parentObjects;

    public void Awake()
    {
        parentSpawnedObjectId = 0;
        ParentPath = "null";
        parentObjects = new Dictionary<uint, SpawnedGameObject>();
    }

    public void SetId(uint _id)
    {
        spawnedObjectId = _id;
    }
}



