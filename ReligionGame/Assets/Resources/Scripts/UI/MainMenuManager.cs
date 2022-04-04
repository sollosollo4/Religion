using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject MainMenu;

    private bool isOpen;
    // Start is called before the first frame update
    void Start()
    {
        MainMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
#else
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOpen)
                CloseMainMenu();
            else
                OpenMainMenu();
        }
#endif
    }

    public void OpenMainMenu()
    {
        MainMenu.gameObject.SetActive(true);
        UIManager.instance.CrosshairGameObject.SetActive(false);
        isOpen = true;
    }

    public void CloseMainMenu()
    {
        MainMenu.gameObject.SetActive(false);
        UIManager.instance.CrosshairGameObject.SetActive(true);
        isOpen = false;
    }

    public void OpenSettings()
    {

    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
