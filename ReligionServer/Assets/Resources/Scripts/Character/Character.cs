using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Character
{
    public int CharacterId;
    public string CharacterName;
    public CharacterClass CharacterClass;
    public int AccountId;

    public static int CreateNewCharacter(Character newChar)
    {
        return Server.mySqlConnection.getController<Assets.Database.Controllers.CharacterController>().createNewCharacter(newChar);
    }

    public static Vector3 GetCharacterWorldPosition(int characterId)
    {
        return Server.mySqlConnection.getController<Assets.Database.Controllers.CharacterWorldInfoController>().getCharacterWorldPosition(characterId);
    }
}

