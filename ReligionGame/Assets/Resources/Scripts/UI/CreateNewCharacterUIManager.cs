using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateNewCharacterUIManager : MonoBehaviour
{
    public static CreateNewCharacterUIManager instance;

    public CreateNewCharacterUI pickedCharacterPrefab;

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

    public void ConnectToServer()
    {
        ClientSend.WelcomeReceived();
        SceneManager.LoadScene("Main");
    }

    public void CreateNew()
    {
        gameObject.SetActive(false);
        pickedCharacterPrefab.gameObject.SetActive(true);
        pickedCharacterPrefab.PickWarrior();
    }
}

