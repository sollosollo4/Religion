using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public float sensitivity = 100f;

    [SerializeField] public Transform cam = null;
    [SerializeField] public Transform orientation = null;

    [SerializeField] public Transform cameraPosition;

    public float clampAngle = 87f;

    float mouseX;
    float mouseY;

    public float multiplier = 0.01f;

    float yRotation;
    float xRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (UIManager.instance.IsClickedUIButton())
            CursorLock();
        

        if(!Cursor.visible)
            Look();
    }

    public void CursorLock()
    {
        if (Cursor.visible)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void Look()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += mouseX * sensitivity * multiplier;
        xRotation -= mouseY * sensitivity * multiplier;

        xRotation = Mathf.Clamp(xRotation, -clampAngle, clampAngle);

        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}
