using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterCell : MonoBehaviour
{
    public Character CharacterOnCell;
    public Text gameText;

    public Text CharacterName;
    public Text CharacterClass;

    public void SetCharacterOnCell(Character chars) {
        CharacterOnCell = chars;
        CharacterName.text = CharacterOnCell.CharacterName;
        CharacterClass.text = CharacterOnCell.CharacterClass.CharacterClassName;
    } 

    private void Start()
    {
        gameText = GameObject.Find("YouSelectedCharacterName").GetComponent<Text>();

        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { SelectCharacterCell((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    public void SelectCharacterCell(PointerEventData data)
    {
        gameText.text = "Вы выбрали: " + CharacterOnCell.CharacterName;
        CreateNewCharacterUIManager.instance.currentSelectCharacterId = CharacterOnCell.CharacterId;
    }
}
