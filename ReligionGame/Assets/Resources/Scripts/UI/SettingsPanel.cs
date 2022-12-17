using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    public Camera GameCamera;

    public Text textView;
    public Text textFar;

    public bool IsOpen;
    public float runTimeFieldOfView;

    public void Start()
    {
        if (runTimeFieldOfView != 0f)
            GameCamera.fieldOfView = runTimeFieldOfView;
    }

    public void UpdateCameraFar()
    {
        if(GetComponentInChildren<Scrollbar>().value == 0)
        {
            GameCamera.farClipPlane = 50;
        }
        else
        {
            Debug.Log(GetComponentInChildren<Scrollbar>().value);
            float newCameraFarValue = GetComponentInChildren<Scrollbar>().value * 300 + 80;
            GameCamera.farClipPlane = newCameraFarValue;
            textFar.GetComponentsInChildren<Text>().Where(f => f.name == "CameraFarValue").First().text = newCameraFarValue.ToString();
        }
        
    }

    public void UpdateCameraView()
    {
        if (GetComponentInChildren<Scrollbar>().value == 0)
        {
            runTimeFieldOfView = 30;
        }
        else
        {
            Debug.Log(GetComponentInChildren<Scrollbar>().value);
            float newCameraViewValue = Mathf.Round(GetComponentInChildren<Scrollbar>().value * 100 + 30);
            runTimeFieldOfView = newCameraViewValue;
            textView.GetComponentsInChildren<Text>().Where(f => f.gameObject != gameObject).First().text = newCameraViewValue.ToString();
        }
    }

    public void Show()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
