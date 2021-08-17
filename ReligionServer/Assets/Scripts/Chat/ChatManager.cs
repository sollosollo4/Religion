using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    public static ChatManager instance;
    public ChatCommand CommandInstance;

    public static readonly string[] ForbiddenWords = { "fuck", "bitch", "cum", "forbidden", "arsehole", "ass", "bullshit", "feck", "pissed", "munter", "bint", "whore" };

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
        CommandInstance = new ChatCommand();
    }

    /****
     * .set_time_night - simple command. set night time at server.
     * .set_new_spawn_enemy_point 8  - medium command. spawn point of enemies with level 8
     * .give_item 0,10,0  - hard command. give item to player with id 0, quantity 10, and level 0
     * .set_new_spawn_enemy_point 8,[4.55,78.11,19.1] - very hard command. spawn point of enemies with level 8, at coordinations x:4.55, y:78.11, z:19.1
     */
    public static void MessageController(string message)
    {
        if(ChatCommand.ChatCommands.Contains(message))
        {
            string[] _params = message.Split(' ')[1].Split(',');
            instance.CommandInstance.ExecuteCommand(message.Split(' ')[0], _params);
        }
        if(ChatManager.ForbiddenWords.Contains(message))
        {
            string []_params = new string[1] { Array.IndexOf(ForbiddenWords, message).ToString() };
            instance.CommandInstance.ExecuteCommand("ForbiddenError", _params);
        }
    }
}
