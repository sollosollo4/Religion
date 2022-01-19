using Assets.Database.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[SerializeField]
public class SpawnedGameObject : MonoBehaviour
{
    [SerializeField] protected uint id;
    
    public ushort PrefabId;
    public ushort parentSpawnedObjectId;
    public string ParentPath;

    public string type;

    public uint spawnedObjectId => id;
    private Dictionary<uint, SpawnedGameObject> parentObjects;

    public virtual void Awake()
    {
        parentSpawnedObjectId = 0;
        ParentPath = "null";
        parentObjects = new Dictionary<uint, SpawnedGameObject>();
    }

    public void SetId(uint _id)
    {
        id = _id;
    }
}



