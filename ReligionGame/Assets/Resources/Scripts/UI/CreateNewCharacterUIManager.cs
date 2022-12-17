using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateNewCharacterUIManager : MonoBehaviour
{
    public static CreateNewCharacterUIManager instance;

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

    public int currentSelectCharacterId;

    public CreateNewCharacterUI pickedCharacterPrefab;
    public GameObject characterCellPrefab;
    public GameObject characterPanel;
    public GameObject errorForm;

    private void Start()
    {
        if (Client.instance.characters != null)
        {
            foreach (Character chars in Client.instance.characters)
            {
                AddCharacterToPanel(chars);
            }
        }
    }

    public void AddCharacterToPanel(Character chars)
    {
        GameObject cell = Instantiate(characterCellPrefab);
        cell.transform.SetParent(characterPanel.transform, false);
        cell.GetComponent<CharacterCell>().SetCharacterOnCell(chars);
    }

    public void ConnectToServer()
    {
        if (currentSelectCharacterId > 0)
        {
            Debug.Log($"Selected character: {currentSelectCharacterId}");
            ClientSend.WelcomeReceived(currentSelectCharacterId);
            SceneManager.LoadScene("Main");
        }
        else
        {
            ErrorForm("Выберите персонажа");
        }
    }

    public void ErrorForm(string error)
    {
        errorForm.SetActive(true);
        errorForm.GetComponentsInChildren<Text>().First().text = error;
    }

    public void CreateNew()
    {
        gameObject.SetActive(false);
        pickedCharacterPrefab.gameObject.SetActive(true);
        pickedCharacterPrefab.PickWarrior();
    }
}

