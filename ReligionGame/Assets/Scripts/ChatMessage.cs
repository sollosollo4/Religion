using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatMessage : MonoBehaviour
{
    public Text UserNameField;
    public Text UserMessageField;

    public void Initialize(int _userId, string _message)
    {
        UserNameField.text = GameManager.players[_userId].username;
        UserMessageField.text = _message;
    }    
}
