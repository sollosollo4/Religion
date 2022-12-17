using Assets.Database.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Database.Controllers
{
    public class CharacterWorldInfoController : Controller
    {
        MySqlConnection connection;
        character_world_info CharacterWorldInfo;
        character_info_class CharacterInfoClass;
        public IDbModel DbModel => CharacterWorldInfo;

        public CharacterWorldInfoController(MySqlConnection connection)
        {
            this.connection = connection;
            CharacterWorldInfo = new character_world_info();
            CharacterWorldInfo.createModel(connection);

            CharacterInfoClass = new character_info_class();
            CharacterInfoClass.createModel(connection);
        }

        public int getCharacterLocation(int characterId)
        {
            return 1000;
        }

        public Vector3 getCharacterWorldPosition(int characterId)
        {
            Vector3 lastPosition = Vector3.zero;

            string getCharacterWorldId = CharacterInfoClass.getAllRowsByFields(new Dictionary<string, string>()
            {
                { "characterId",  characterId.ToString() }
            });
            connection.Open();
            MySqlCommand cmd1 = new MySqlCommand(getCharacterWorldId, connection);
            MySqlDataReader reader1 = cmd1.ExecuteReader();
            CharacterInfoClass = (character_info_class)(CharacterInfoClass.GetModel(reader1).First());
            connection.Close();

            string getPositionQuery = CharacterWorldInfo.getAllRowsByFields(new Dictionary<string, string>() { { "id", CharacterInfoClass.characterWorldInfoId.ToString() } }, ESelectableMethod.Equal);
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(getPositionQuery, connection);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                CharacterWorldInfo = (character_world_info)(CharacterWorldInfo.GetModel(reader).First());
                lastPosition.x = CharacterWorldInfo.characterWorldInfoPosX;
                lastPosition.y = CharacterWorldInfo.characterWorldInfoPosY;
                lastPosition.z = CharacterWorldInfo.characterWorldInfoPosZ;
            }

            connection.Close();
            return lastPosition;
        }

        public void setCharacterWorldPosition(Vector3 savePosition, int _characterId)
        {
            string getCharacterWorldId = CharacterInfoClass.getAllRowsByFields(new Dictionary<string, string>()
            {
                { "characterId",  _characterId.ToString() }
            });
            connection.Open();
            MySqlCommand cmd1 = new MySqlCommand(getCharacterWorldId, connection);
            MySqlDataReader reader1 = cmd1.ExecuteReader();
            CharacterInfoClass = (character_info_class)(CharacterInfoClass.GetModel(reader1).First());
            connection.Close();

            string setPositionQuery = CharacterWorldInfo.updateRowWithValuesByFields(
                new Dictionary<string, object>() {
                    { "characterWorldInfoPosX", savePosition.x.ToString().Replace(',', '.') },
                    { "characterWorldInfoPosY", (savePosition.y+2f).ToString().Replace(',', '.') },
                    { "characterWorldInfoPosZ", savePosition.z.ToString().Replace(',', '.') }
                }, 
                new Dictionary<string, string>() {
                    { "id", CharacterInfoClass.characterWorldInfoId.ToString() }
                });
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(setPositionQuery, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}
