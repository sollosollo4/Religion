using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public float health;
    public float maxHealth = 100f;
    public int itemCount = 0;
    public byte animationState = 0;
    public SkinnedMeshRenderer model;
    public Animator animator;

    public bool IsTool;
    public bool IsSprint;

    public string WorkTouchName;
    public TouchableStructures lastTouchableStructure;

    public GameObject ToolHand;
    public GameObject CurrentWorkTool;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;

        SetPlayerInfoView();
    }

    public void InitializeLocale(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;

        SetPlayerInfoView();
        //ActiveChatPanel();
        SetGlowEffectsObjects();
    }

    private void SetGlowEffectsObjects()
    {
        foreach (var obj in GameManager.instance.scriptableObjectPrefab.GetComponentsInChildren<SpawnedGameObject>())
        {
            GlowController.RegisterObject(obj.GetComponent<GlowObjectCmd>());
        }
    }

    public void SetWorkName(string touchEventName)
    {
        WorkTouchName = touchEventName;
    }

    public void SetPlayerInfoView()
    {
        if (id != Client.instance.myId) 
        {
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
