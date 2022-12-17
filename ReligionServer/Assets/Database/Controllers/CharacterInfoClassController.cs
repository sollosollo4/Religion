using Assets.Database.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Database.Controllers
{
    public class CharacterInfoClassController : Controller
    {
        MySqlConnection connection;
        character_info_class CharacterInfoClass;
        public IDbModel DbModel => CharacterInfoClass;

        public CharacterInfoClassController(MySqlConnection connection)
        {
            this.connection = connection;
            CharacterInfoClass = new character_info_class();
            CharacterInfoClass.createModel(connection);
        }

        public character_info_class getInfo(int characterId)
        {
            string getInfoQuery = CharacterInfoClass.getAllRowsByFields(new Dictionary<string, string>() 
                { 
                    { "characterId", characterId.ToString() } 
                }, 
                ESelectableMethod.Equal, 1
            );
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(getInfoQuery, connection);
            using MySqlDataReader reader = cmd.ExecuteReader();
            CharacterInfoClass.GetModel(reader);
            connection.Close();

            return CharacterInfoClass;
        }
    }
}
