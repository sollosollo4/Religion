using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public float health;
    public float maxHealth = 100f;
    public int itemCount = 0;
    public MeshRenderer model;

    public GameObject objectViewInfo;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;

        SetPlayerInfoView();
        ActiveChatPanel();
    }

    public void SetPlayerInfoView()
    {
        if (id != Client.instance.myId) { 
            objectViewInfo.GetComponent<ObjectInfoView>().text = username;
        }
    }

    public void ActiveChatPanel()
    {
        UIManager.instance.ChatPanel.SetActive(true);
    }

    public void SetHealth(float _health)
    {
        health = _health;

        if (health <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        model.enabled = false;
    }

    public void Respawn()
    {
        model.enabled = true;
        SetHealth(maxHealth);
    }
}
