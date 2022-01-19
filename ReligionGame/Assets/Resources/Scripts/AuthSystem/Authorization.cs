using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Authorization : MonoBehaviour
{
    public static Authorization instance;

    public InputField loginInput;
    public InputField passwordInput;
    public GameObject errorForm;

    public Image backGroundImage;

    private int changeColorState;

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

        changeColorState = 0;

        backGroundImage.color = new Color(0, 0, 255, 255);
        StartCoroutine(changeColorBackround());
    }

    IEnumerator changeColorBackround()
    {
        while (true)
        {
            float 
                r = backGroundImage.color.r, 
                g = backGroundImage.color.g, 
                b = backGroundImage.color.b;
            Debug.Log($"{r} {g} {b}");
            backGroundImage.color = getNewColor(r ,g, b);
            yield return new WaitForSeconds(0.5f);   
        }
    }

    Color getNewColor(float r, float g, float b)
    {
        switch(changeColorState)
        {
            // from full b to full g
            case 0:
                g += 17f;
                if (g == 255) changeColorState = 1;
                return new Color(r, g, b, 255);
            // from full g to null b
            case 1:
                b -= 17f;
                if (b == 0) changeColorState = 2;
                return new Color(r, g, b, 255);
            // from full g to full r
            case 2:
                r += 17f;
                if (r == 255) changeColorState = 3;
                return new Color(r, g, b, 255);
            // from full r to null g
            case 3:
                g -= 17f;
                if (g == 0) changeColorState = 4;
                return new Color(r, g, b, 255);
            // from full r to full b
            case 4:
                b += 17f;
                if (b == 255) changeColorState = 5;
                return new Color(r, g, b, 255);
            // from full b to null r
            case 5:
                r -= 17f;
                if (r == 0) changeColorState = 0;
                return new Color(r, g, b, 255);
            default:
                return new Color(r, g, b, 255);
        }
    }

    public void TryConnectToServer()
    {
        Client.instance.ConnectToServer();
        
        StartCoroutine(ExecuteAfterTime(1f));
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        ClientAuthSend.PlayerTryConnection(loginInput.text, passwordInput.text);
    }

    public void LoadCreateNewCharacterScene(bool _isConnectAccess, string _message)
    {
        if(_isConnectAccess)
        {
            SceneManager.LoadScene("CreateNewCharacter");
        }
        else
        {
            ShowErrorForm(_message);
            Client.instance.Disconnect();
        }
    }

    public void ShowErrorForm(string _message)
    {
        errorForm.SetActive(true);
        errorForm.GetComponentInChildren<Text>().text = _message;
    }

    public void CloseErrorForm()
    {
        errorForm.SetActive(false);
    }
}
