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

    public void TryConnectToServer()
    {
        if(!Client.instance.isValid())
            Client.instance.ConnectToServer();

        StartCoroutine(ExecuteAfterTime(1));
    }
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        ClientAuthSend.PlayerTryConnection(loginInput.text, passwordInput.text);
    }

    public void ConnectToServer(bool _isConnectAccess, string _message)
    {
        if(_isConnectAccess)
        {
            SceneManager.LoadScene("CreateNewCharacter");
        }
        else
        {
            ShowErrorForm(_message);
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
