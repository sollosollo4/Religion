using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject CrosshairGameObject;

    public GameObject ChatPanel;
    public GameObject gameMenu;
    public GameObject toolPanel;
    public GameObject inventoryPanel;

    private void Awake()
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

    public void OpenInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        inventoryPanel.GetComponent<Inventory>().IsOpen = inventoryPanel.activeSelf;
        CrosshairGameObject.SetActive(!CrosshairGameObject.activeSelf);
    }
}
