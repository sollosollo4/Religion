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
        switch (characterClass)
        {
            case "warrior":
                return new CharacterClass()
                {
                    CharacterClassCode = "warrior",
                    CharacterClassName = "Warrior",
                    CharacterClassDescription = "Sword and sword"
                };
            case "druid":
                return new CharacterClass()
                {
                    CharacterClassCode = "druid",
                    CharacterClassName = "Druid",
                    CharacterClassDescription = "Wand! Only wand"
                };
            case "hunter":
                return new CharacterClass()
                {
                    CharacterClassCode = "hunter",
                    CharacterClassName = "Hunter",
                    CharacterClassDescription = "Bow and arrows.."
                };
            case "mage":
                return new CharacterClass()
                {
                    CharacterClassCode = "mage",
                    CharacterClassName = "Mage",
                    CharacterClassDescription = "Books and magic"
                };
            case "rogue":
                return new CharacterClass()
                {
                    CharacterClassCode = "rogue",
                    CharacterClassName = "Rogue",
                    CharacterClassDescription = "Be careful, if he got knives"
                };
            default:
                return new CharacterClass()
                {
                    CharacterClassCode = "warrior",
                    CharacterClassName = "Warrior",
                    CharacterClassDescription = "Sword and sword"
                };
        }
    }
}

