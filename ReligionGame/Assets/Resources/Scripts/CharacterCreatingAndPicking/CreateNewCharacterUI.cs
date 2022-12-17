using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreateNewCharacterUI : MonoBehaviour
{
    public static CreateNewCharacterUI instance;
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

    [SerializeField]
    public Text characterClassDescription;

    [SerializeField] public List<GameObject> characterPickersImages;

    [SerializeField] public InputField inputCharacterNickname;

    [SerializeField] public GameObject errorForm;

    [SerializeField] public GameObject loginToServerUI;

    public string selectedCharacterClass;

    public void CancelCreate()
    {
        gameObject.SetActive(false);
        loginToServerUI.SetActive(true);
    }

    public void CreateNew()
    {
        if (inputCharacterNickname.text.Length >= 4)
            ClientSend.CreateNewCharater(inputCharacterNickname.text, selectedCharacterClass);
    }

    public static void CloseCreateFormAndAddNewCharacterToList(Character character)
    {
        instance.CancelCreate();
        CreateNewCharacterUIManager.instance.AddCharacterToPanel(character);
    }

    public void ErrorForm(string error)
    {
        errorForm.SetActive(true);
        errorForm.GetComponentsInChildren<Text>().First().text = error;
    }

    public void CloseErrorForm()
    {
        errorForm.SetActive(false);
    }


    public void PickWarrior()
    {
        characterPickersImages.ForEach(x => { x.GetComponent<Image>().color = new Color(0, 0, 0, 0); });
        characterPickersImages[0].GetComponent<Image>().color = new Color(0, 0, 0, 141);
        selectedCharacterClass = "warrior";
    }

    public void PickDruid()
    {
        characterPickersImages.ForEach(x => { x.GetComponent<Image>().color = new Color(0, 0, 0, 0); });
        characterPickersImages[1].GetComponent<Image>().color = new Color(0, 0, 0, 141);
        selectedCharacterClass = "druid";
    }

    public void PickHunter()
    {
        characterPickersImages.ForEach(x => { x.GetComponent<Image>().color = new Color(0, 0, 0, 0); });
        characterPickersImages[2].GetComponent<Image>().color = new Color(0, 0, 0, 141);
        selectedCharacterClass = "hunter";
    }

    public void PackMage()
    {
        characterPickersImages.ForEach(x => { x.GetComponent<Image>().color = new Color(0, 0, 0, 0); });
        characterPickersImages[3].GetComponent<Image>().color = new Color(0, 0, 0, 141);
        selectedCharacterClass = "mage";
    }

    public void PickRogue()
    {
        characterPickersImages.ForEach(x => { x.GetComponent<Image>().color = new Color(0, 0, 0, 0); });
        characterPickersImages[4].GetComponent<Image>().color = new Color(0, 0, 0, 141);
        selectedCharacterClass = "rogue";
    }
}
