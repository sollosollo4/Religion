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

    public string WorkTouchName;
    public TouchableStructures lastTouchableStructure;

    public GameObject ToolHand;
    public GameObject CurrentWorkTool;

    public void Initialize(int _id, string _username)
    {
        if(Client.instance.myId == _id)
            GetComponentInChildren<SkinnedMeshRenderer>().gameObject.SetActive( false );
        id = _id;
        username = _username;
        health = maxHealth;


        SetPlayerInfoView();
        ActiveChatPanel();
        SetGlowEffectsObjects();
        animator.Play("Idle01");
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

    public void SetPlayerMovementStateAnimation(Dictionary<string, bool> inputs)
    {
        bool singleAnimation = inputs.Values.Where(x => x == true).Count() == 1;
        bool noOneAnimation = inputs.Values.All(x => !x);

        if (noOneAnimation)
        {
            SetState(0);
        }
        else if(singleAnimation)
        {
            foreach (var animationInput in inputs)
            {
                if (animationInput.Value)
                {
                    if(animationInput.Key == "Jump")
                    {
                        SetState(animationInput.Key);
                    }
                    else
                    {
                        SetState(animationInput.Key);
                    }
                }
            }
        }
        else
        {
            string AnimationNameConcat = "";
            foreach (var animationInput in inputs)
            {
                if (animationInput.Value)
                {
                    if (animationInput.Key == "Jump")
                    {
                        SetState(animationInput.Key);
                        AnimationNameConcat += animationInput.Key;
                    }
                    else
                    {
                         AnimationNameConcat += animationInput.Key;
                    }
                }
            }
            SetState(AnimationNameConcat);
        }
    }
    
    public void SetWork(Dictionary<string, bool> inputsAnimation)
    {
        foreach (var animationInput in inputsAnimation)
        {
            if (animationInput.Value && WorkTouchName != string.Empty && lastTouchableStructure.PlayerRaycast(Camera.main))
            {
                IsTool = true;
                SetState(WorkTouchName);
                lastTouchableStructure.StartMining();
            }
        }
    }

    public void SetState(byte state)
    {
        animationState = state;

        if (IsTool)
        {
            animator.Play(WorkTouchName);
        }
        else
        {
            animator.Play(GetAnimationStateName(animationState));
        }
    }

    public void SetState(string state)
    {
        animationState = GetAnimationStateName(state);

        if (IsTool)
        {
            animator.Play(WorkTouchName);
        }
        else
        {
            animator.Play(GetAnimationStateName(animationState));
        }
    }
    
    public static string GetAnimationStateName(byte state)
    {
        switch (state)
        {
            case 0:
                return "Idle01";
            case 1:
                return "Forward";
            case 2:
                return "Backward";
            case 3:
                return "Left";
            case 4:
                return "Right";
            case 5:
                return "ForwardLeft";
            case 6:
                return "ForwardRight";
            case 7:
                return "BackwardLeft";
            case 8:
                return "BackwardRight";
            case 9:
                return "Jump";
            case 10: 
                return "Pickaxe";
            default:
                return "Idle01";
        }
    }

    public static byte GetAnimationStateName(string state)
    {
        switch (state)
        {
            case "Idle01":
                return 0;
            case "Forward":
                return 1;
            case "Backward":
                return 2;
            case "Left":
                return 3;
            case "Right":
                return 4;
            case "ForwardLeft":
                return 5;
            case "ForwardRight":
                return 6;
            case "BackwardLeft":
                return 7;
            case "BackwardRight":
                return 8;
            case "Jump":
                return 9;
            case "Pickaxe":
                return 10;
            default:
                return 0;
        }
    }
}
