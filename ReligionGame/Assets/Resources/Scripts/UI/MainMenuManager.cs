using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject SettingsPanel;
    public bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ImStuck()
    {
        ClientSend.PlayerStuck();
    }

    public void OpenSettings()
    {
        SettingsPanel.GetComponent<SettingsPanel>().Show();
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void CloseAllForms()
    {
        OpenSettings();
    }
}
