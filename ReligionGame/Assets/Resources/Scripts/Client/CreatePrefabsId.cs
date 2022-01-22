#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StructuresPrefabsId))]
public class CreatePrefabsId : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StructuresPrefabsId myLoading = (StructuresPrefabsId)target;

        if (GUILayout.Button("Build prefab ids"))
        {
            myLoading.Loading();
        }
    }
}
#endif
