using Assets.Database.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[Serializable]
public class StructuresPrefabsId : ScriptableObject
{
    public Dictionary<ushort, string> prefabIdToPath;

    [MenuItem("Tools/MySql/Generate ids for prefabs")]
    public static void GiveAllObjectsAUniqueID()
    {

        uint id = 0;

        SpawnedGameObject[] touchableObjects = FindObjectsOfType<SpawnedGameObject>();
        foreach (SpawnedGameObject obj in touchableObjects)
        {
            obj.SetId(id);
            id++;
        }

        Server.mySqlConnection = new Assets.Database.MySqlConnectionClass();
        Server.mySqlConnection.getController<StructureController>().savePrefabs(touchableObjects);
    }

    public static IEnumerator BuildPrefabPoolCoroutine()
    {
        ushort _prefabId = 1;
        var loadPath = $"{Application.dataPath}/Resources/SpawnablePrefabs";
        string[] spawnablePrefabs = Directory.GetFiles(loadPath, "*.prefab", SearchOption.AllDirectories);
        List<string> prefabData = new List<string>();
        for (int i = 0; i < spawnablePrefabs.Length; i++)
        {
            string prefabPath = spawnablePrefabs[i];
            var path = prefabPath.Replace($"{Application.dataPath}/Resources/", "").Replace("\\", "/").Replace(".prefab", "");
            var spawnablePrefab = Resources.Load<GameObject>(path);
            if (spawnablePrefab == null) { Debug.LogError($"NO PREFAB AT PATH: {path}"); continue; }
            Debug.Log($"Success load: {spawnablePrefab.name}");
            var spawnedObject = spawnablePrefab.GetComponent<SpawnedGameObject>();
            if (spawnedObject == null)
            {
                Debug.LogError($"{spawnablePrefab} is missing a SpawnedGameObject. Intentional?");
                continue;
            }

            if (spawnedObject.PrefabId != _prefabId)
            {
                spawnedObject.PrefabId = _prefabId;
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
