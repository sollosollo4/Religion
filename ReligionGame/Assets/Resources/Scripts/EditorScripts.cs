using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorScripts : MonoBehaviour
{
    #if UNITY_EDITOR
    [MenuItem("Tools/Play/PlayMe _%h")]
    public static void RunMainScene()
    {
        EditorApplication.OpenScene("Assets/Scenes/Authorization.unity");
        EditorApplication.isPlaying = true;
    }

    [MenuItem("Tools/Play/OpenMainChene _%j")]
    public static void OpenMainScene()
    {
        EditorApplication.OpenScene("Assets/Scenes/Main.unity");
    }
#endif
}
