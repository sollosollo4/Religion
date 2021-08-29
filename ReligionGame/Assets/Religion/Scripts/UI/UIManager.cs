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

    public GameObject ChatPanel;
    public GameObject startMenu;
    public InputField usernameField;

    public CharacterPickerObject pickedCharacter;

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

    /// <summary>Attempts to connect to the server.</summary>
    public void ConnectToServer()
    {
        //startMenu.SetActive(false);
        //usernameField.interactable = false;
        if(!Client.instance.isValid())
            Client.instance.ConnectToServer();

        ClientSend.WelcomeReceived();
        SceneManager.LoadScene("Main");
    }
}
