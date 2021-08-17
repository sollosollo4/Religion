using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public static ChatManager instance;


    public static Dictionary<int, List<ChatMessage>> messages = new Dictionary<int, List<ChatMessage>>();
    public static string getCurrentMessage;

    public GameObject chatPanel;
    public GameObject chatMessagePrefab;
    public InputField chatPlayerMessageInputField;

    public static bool isPlayerTypingMessage;

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

    private void Start()
    {
        chatPlayerMessageInputField.onValueChanged.AddListener(delegate { getCurrentMessage = chatPlayerMessageInputField.text.ToString(); });
    }

    private void Update()
    {
        try
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (!string.IsNullOrEmpty(getCurrentMessage) && isPlayerTypingMessage)
                {
                    CreateChatMessageAndSend();
                }
                else
                {
                    OpenMessageInput();
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
    }

    void OpenMessageInput()
    {
        if (isPlayerTypingMessage)
        {
            CloseChatMessageInput();
        }
        else
        {
            isPlayerTypingMessage = true;
            chatPlayerMessageInputField.gameObject.SetActive(isPlayerTypingMessage);
            chatPlayerMessageInputField.Select();
            chatPlayerMessageInputField.ActivateInputField();

            CameraController.instance.ToggleCursorMode();
        }
    }

    void CloseChatMessageInput()
    {
        getCurrentMessage = null;
        isPlayerTypingMessage = false;
        chatPlayerMessageInputField.gameObject.SetActive(isPlayerTypingMessage);
        chatPlayerMessageInputField.DeactivateInputField();
        CameraController.instance.ToggleCursorMode();
    }
    

    void CreateChatMessageAndSend()
    {
        // ?? ar we need CreateChatMessage call on client? or not?
        ClientSend.PlayerSendChatMessage(getCurrentMessage);
        CloseChatMessageInput();
    }

    public void CreateChatMessage(int _userId, string _message)
    {
        GameObject _chatMessage = Instantiate(chatMessagePrefab, chatPanel.transform);
        _chatMessage.GetComponent<ChatMessage>().Initialize(_userId, _message);
        if (messages.ContainsKey(_userId))
            messages[_userId].Add(_chatMessage.GetComponent<ChatMessage>());
        else
            messages.Add(_userId, new List<ChatMessage>() { _chatMessage.GetComponent<ChatMessage>() });
    }
}
