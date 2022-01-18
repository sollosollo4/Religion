using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Character
{
    public string CharacterName;
    public CharacterClass CharacterClass;
    public int AccountId;

    public static bool CreateNewCharacter(Character newChar)
    {
        return Server.mySqlConnection.getController<Assets.Database.Controllers.CharacterController>().createNewCharacter(newChar);
    }
}

