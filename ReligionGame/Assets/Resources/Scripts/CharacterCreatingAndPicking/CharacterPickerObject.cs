using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPickerObject : MonoBehaviour
{
    public Text CharacterName;
    public Text CharacterClassName;
    public Text CharacterLastLocationName;

    public Button characterPicked;

    public CharacterPickerObject(string _name, string _classname, string _last_location)
    {
        CharacterName.text = _name;
        CharacterClassName.text = _classname;
        CharacterLastLocationName.text = _last_location;
    }
}
