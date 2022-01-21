using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class StructuresPrefabsId : MonoBehaviour
{
    public Dictionary<ushort, string> prefabIdToPath;
    public void Loading()
    {
        LoadData();

        uint id = 0;
        SpawnedGameObject[] touchableObjects = FindObjectsOfType<SpawnedGameObject>();
        foreach (SpawnedGameObject obj in touchableObjects)
        {
            Debug.Log(obj.gameObject.name + obj.GetInstanceID());
            obj.SetId(id);
            id++;

            EditorUtility.SetDirty(obj);
            AssetDatabase.SaveAssets();
        }
    }

    public void ImportData()
    {
        string[] iconData = File.ReadAllLines($"{Application.dataPath}/Resources/prefabIds.txt");
        prefabIdToPath = new Dictionary<ushort, string>();
        foreach (var iconValue in iconData)
        {
            var splitValue = iconValue.Split('|');
            ushort prefabId = ushort.Parse(splitValue[0]);
            string prefabPath = splitValue[1];
            prefabIdToPath.Add(prefabId, prefabPath);
        }
    }
    public void LoadData()
    {
        if (prefabIdToPath == null || prefabIdToPath.Count == 0) ImportData();
        foreach (var prefabPath in prefabIdToPath)
        {
            var prefab = Resources.Load<GameObject>(prefabPath.Value);
            SpawnedGameObject spawnedGameObject = prefab.GetComponent<SpawnedGameObject>();
            if (spawnedGameObject == null)
            {
                Debug.LogError($"No SpawnedGameObject @ {prefabPath.Value}");
                continue;
            }

            if (spawnedGameObject.PrefabId != prefabPath.Key)
            {
                spawnedGameObject.PrefabId = prefabPath.Key;
                EditorUtility.SetDirty(prefab);
                AssetDatabase.SaveAssets();
            }

        }
    }
}
