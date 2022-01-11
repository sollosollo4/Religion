using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

public class CharacterClass
{
    public string CharacterClassCode;
    public string CharacterClassName;
    public string CharacterClassDescription;

    public Image CharacterClassIcon;

    public static CharacterClass CreateClassByName(string characterClass)
    {
        return new CharacterClass();
    }
}

