using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AltSkillPanel : MonoBehaviour, IHasChanged
{
    public delegate SkillUI SkillClick(SkillUI sender, SkillEventArgs e);
    public event SkillClick SkillClickEvent;

    public List<SkillUI> SkillButtons;
    bool Visible;

    void Start()
    {
        SkillButtons = new List<SkillUI>(9);
        SkillButtons = GetComponentsInChildren<SkillUI>().ToList();

        SetTransparentColor();
        Visible = false;

        HasChanged();
    }
    public void HasChanged()
    {

    }

    void Update()
    {
        // Show/Hide panel
        if(Input.GetKey(KeyCode.LeftAlt) && !Visible)
        {
            Visible = true;
            SetActiveColor();
        }
        if(!Input.GetKey(KeyCode.LeftAlt) && Visible)
        {
            Visible = false;
            SetTransparentColor();
        }

        // Clicked Buttons
        if(Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha1))
        {
            SkillClicked(SkillButtons[0], 1f);
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha2))
        {
            SkillClicked(SkillButtons[1], 1f);
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha3))
        {
            SkillClicked(SkillButtons[2], 1f);
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha4))
        {
            SkillClicked(SkillButtons[3], 1f);
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha5))
        {
            SkillClicked(SkillButtons[4], 1f);
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha6))
        {
            SkillClicked(SkillButtons[5], 1f);
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha7))
        {
            SkillClicked(SkillButtons[6], 1f);
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha8))
        {
            SkillClicked(SkillButtons[7], 1f);
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha9))
        {
            SkillClicked(SkillButtons[8], 1f);
        }
    }
    protected virtual void SkillClicked(SkillUI sender, float cooldown)
    {
        Debug.Log(sender.name+"   "+ cooldown);
        SkillClickEvent?.Invoke(sender, new SkillEventArgs(cooldown));
        GlobalCooldown();
        StartCooldown(cooldown);
    }

    private void GlobalCooldown()
    {

    }

    private void StartCooldown(float cooldown)
    {

    }

    void SetTransparentColor()
    {
        foreach (Image transparrent in GetComponentsInChildren<Image>())
        {
            transparrent.color = new Color(255/255, 255 / 255, 255 / 255, 5f/255);
        }
        foreach (Text transparrent in GetComponentsInChildren<Text>())
        {
            transparrent.color = new Color(50 / 255, 50 / 255, 50 / 255, 30f / 255);
        }
    }

    void SetActiveColor()
    {
        foreach (Image transparrent in GetComponentsInChildren<Image>())
        {
            transparrent.color = new Color(255 / 255, 255 / 255, 255 / 255, 1f);
        }
        foreach (Text transparrent in GetComponentsInChildren<Text>())
        {
            transparrent.color = new Color(0, 0, 0, 1f);
        }
    }
}
