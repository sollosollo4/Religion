using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPickerObject : MonoBehaviour
{
    public string CharacterName;
    public string CharacterClassName;
    public string CharacterLastLocationName;

    public Button characterPicked;

    public CharacterPickerObject(string _name, string _classname, string _last_location)
    {
        CharacterName = _name;
        CharacterClassName = _classname;
        CharacterLastLocationName = _last_location;
    }
}
