using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public delegate void UIKeyCodeHandler(bool open);
    public delegate bool UIPartHandler();
    public Dictionary<KeyCode, UIKeyCodeHandler> UIKeyCodes;
    public Dictionary<KeyCode, UIPartHandler> IsOpenUIPart;
    public static UIManager instance;

    public GameObject CrosshairGameObject;

    public GameObject ChatPanel;
    public GameObject gameMenu;
    public GameObject toolPanel;
    public GameObject inventoryPanel;
    public GameObject characterPanel;

    public bool IsUIBlock { get; set; }

    private KeyCode MainMenuKeyCode = KeyCode.Numlock;

    private void Start()
    {
        IsUIBlock = false;
        UIKeyCodes = new Dictionary<KeyCode, UIKeyCodeHandler>
        {
            { MainMenuKeyCode, MainMenuAction },
            { KeyCode.B, InventoryAction },
            { KeyCode.C, CharacterAction }
        };

        IsOpenUIPart = new Dictionary<KeyCode, UIPartHandler>
        {
            { MainMenuKeyCode, IsOpenMainMenu },
            { KeyCode.B, IsOpenInventory },
            { KeyCode.C, IsOpenCharacter }
        };
    }

    private void Update()
    {
        IsClickedUIButton();
    }

    public void CloseAll()
    {
        foreach (KeyCode simple in UIKeyCodes.Keys)
        {
            if (IsOpenUIPart[simple]())
            {
                // Закроем этот открытый интерфейс
                UIKeyCodes[simple](false);
            }
        }

        gameMenu.GetComponent<MainMenuManager>().CloseAllForms();
    }

    public bool IsAnyUIPartOpen()
    {
        return IsOpenUIPart.Values.Where(h => h() == true).Count() > 0;
    }

    public bool IsOpenCharacter()
    {
        return characterPanel.activeSelf;
    }

    public bool IsOpenInventory()
    {
        return inventoryPanel.GetComponent<Inventory>().IsOpen;
    }

    public bool IsOpenMainMenu()
    {
        return gameMenu.GetComponent<MainMenuManager>().isOpen;
    }

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

    public bool IsClickedUIButton()
    {
        foreach (KeyCode simple in UIKeyCodes.Keys)
        {
            if (Input.GetKeyDown(simple))
            {
                if (simple == MainMenuKeyCode)
                {
                    UIKeyCodes[simple]( !IsOpenUIPart[simple]() );
                    return true;
                }
                else
                {
                    if (!IsUIBlock)
                    {
                        // Нажали кнопку, и при этом, окно закрыто. Иначе будет переход на стандартный набор.
                        // и не главное меню
                        UIKeyCodes[simple]( !IsOpenUIPart[simple]() );
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void InventoryAction(bool open)
    {
        inventoryPanel.SetActive(open);
        inventoryPanel.GetComponent<Inventory>().IsOpen = open;
    }

    public void CharacterAction(bool open)
    {
        characterPanel.SetActive(open);
    }

    public void MainMenuAction(bool open)
    {
        Debug.Log("Открываем главное меню");
        // Просто закрываем
        if (IsAnyUIPartOpen() && open)
        {
            Debug.Log("Сначала все закроем");
            CloseAll();
        }
        else
        {
            Debug.Log("Блочим другие окна, и показываем главное меню: "+ open);
            // А потом при еще одном нажатии уже открываем меню
            IsUIBlock = open;
            gameMenu.SetActive(open);
            gameMenu.GetComponent<MainMenuManager>().isOpen = open;
        }
    }
}
