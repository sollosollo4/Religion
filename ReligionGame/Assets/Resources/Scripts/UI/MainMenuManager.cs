using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject CrosshairGameObject;
    public GameObject MainMenu;

    private bool isOpen;
    // Start is called before the first frame update
    void Start()
    {
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
        CrosshairGameObject.SetActive(false);
        isOpen = true;
    }

    public void CloseMainMenu()
    {
        MainMenu.gameObject.SetActive(false);
        CrosshairGameObject.SetActive(true);
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
