#if UNITY_EDITOR
using Assets.Database.Controllers;
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
        StartCoroutine(BuildPrefabPoolCoroutine());
        LoadData();

        uint id = 0;
        SpawnedGameObject[] touchableObjects = FindObjectsOfType<SpawnedGameObject>();
        foreach (SpawnedGameObject obj in touchableObjects)
        {
            obj.SetId(id);
            id++;

            EditorUtility.SetDirty(obj);
            AssetDatabase.SaveAssets();
        }

        Server.mySqlConnection = new Assets.Database.MySqlConnectionSingletone();
        Server.mySqlConnection.getController<StructureController>().savePrefabs(touchableObjects);
    }

    public IEnumerator BuildPrefabPoolCoroutine()
    {
        var _prefabId = 1;
        var loadPath = $"{Application.dataPath}/Resources/SpawnablePrefabs";
        string[] spawnablePrefabs = Directory.GetFiles(loadPath, "*.prefab", SearchOption.AllDirectories);
        List<string> prefabData = new List<string>();
        for (int i = 0; i < spawnablePrefabs.Length; i++)
        {
            string prefabPath = spawnablePrefabs[i];
            var path = prefabPath.Replace($"{Application.dataPath}/Resources/", "").Replace("\\", "/").Replace(".prefab", "");
            var spawnablePrefab = Resources.Load<GameObject>(path);
            if (spawnablePrefab == null) { Debug.LogError($"NO PREFAB AT PATH: {path}"); continue; }
            var spawnedObject = spawnablePrefab.GetComponent<SpawnedGameObject>();
            if (spawnedObject == null)
            {
                Debug.LogError($"{spawnablePrefab} ithe buis missing a SpawnedGameObject. Intentional?");
                continue;
            }

            prefabData.Add($"{_prefabId}|{path}");
            _prefabId++;

            var packetIndex = i / 500f;
            if ((packetIndex % 1) == 0)
            {
                Debug.LogError($"BuildPrefabPool - Completed {i} Prefabs.");
                yield return new WaitForSecondsRealtime(.5f);
            }
        }

        File.WriteAllLines($"{Application.dataPath}/Resources/prefabIds.txt", prefabData);

        yield break;
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
#endif