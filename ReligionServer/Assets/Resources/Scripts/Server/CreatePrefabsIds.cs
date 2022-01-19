using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class CreatePrefabsIds : MonoBehaviour
{
    public static CreatePrefabsIds instance;

    public void Update()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }

    }

    [MenuItem("Tools/Create prefabs ids")]
    public static void CreatePrefabsIdsMethod()
    {
        instance.StartCoroutine(StructuresPrefabsId.BuildPrefabPoolCoroutine());

        StructuresPrefabsId structures = (StructuresPrefabsId)ScriptableObject.CreateInstance("StructuresPrefabsId");
        structures.LoadData();
    }
}

