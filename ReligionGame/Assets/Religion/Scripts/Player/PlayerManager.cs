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
    public int animationState = 0;
    public SkinnedMeshRenderer model;
    public Animator animator;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;

        SetPlayerInfoView();
        ActiveChatPanel();
        animator.Play("Idle01");
    }

    public void SetPlayerInfoView()
    {
        if (id != Client.instance.myId) { 
            //GetComponent<ObjectInfoView>().text = username;
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

    public void SetPlayerStateAnimation(Dictionary<string, bool> inputs)
    {
        bool singleAnimation = inputs.Values.Distinct().Count() == 1;
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
                    SetState(animationInput.Key);
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
                    AnimationNameConcat += animationInput.Key;
                }
            }
            SetState(AnimationNameConcat);
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

    public void SetState(int state)
    {
        animationState = state;
        Debug.Log($"NO ONE Animation: {GetAnimationStateName(state)}");
        animator.Play(GetAnimationStateName(state));
        
    }

    public void SetState(string state)
    {
        animationState = GetAnimationStateName(state);
        Debug.Log($"Animation: {state}");
        animator.Play(GetAnimationStateName(animationState));
    }

    public IEnumerator PlayAndWaitForAnim(Animator targetAnim, string stateName)
    {
        int animHash = Animator.StringToHash("Base."+stateName);

        Debug.Log(targetAnim.GetCurrentAnimatorStateInfo(0).fullPathHash);
        targetAnim.CrossFadeInFixedTime(stateName, 0.6f, 0);
        Debug.Log(targetAnim.GetCurrentAnimatorStateInfo(0).fullPathHash);

        while (targetAnim.GetCurrentAnimatorStateInfo(0).fullPathHash != animHash)
        {
            yield return null;
        }

        float counter = 0;
        float waitTime = targetAnim.GetCurrentAnimatorStateInfo(0).length;

        while (counter < (waitTime))
        {
            counter += Time.deltaTime;
            yield return null;
        }

        //Done playing. Do something below!
        Debug.Log("Done Playing");

    }


    public static string GetAnimationStateName(int state)
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
            default:
                return "Idle01";
        }
    }

    public static int GetAnimationStateName(string state)
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
            case "ForwardJump":
                return 9;
            case "BackwardJump":
                return 9;
            case "LeftJump":
                return 9;
            case "RightJump":
                return 9;
            case "ForwardLeftJump":
                return 9;
            case "ForwardRightJump":
                return 9;
            case "BackwardLeftJump":
                return 9;
            case "BackwardRightJump":
                return 9;
            default:
                return 0;
        }
    }
}
