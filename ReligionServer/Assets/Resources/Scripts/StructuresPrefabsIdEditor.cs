using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StructuresPrefabsId))]
public class StructuresPrefabsIdEditor : Editor
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
