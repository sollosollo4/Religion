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
    public class CharacterController : Controller
    {
        MySqlConnection connection;
        characters CharactersModel;
        character_world_info CharacterWorldInfo;
        character_info_class CharacterInfoClass;
        public IDbModel DbModel => CharactersModel;

        public CharacterController(MySqlConnection connection)
        {
            this.connection = connection;
            CharactersModel = new characters();
            CharactersModel.createModel(connection);

            CharacterWorldInfo = new character_world_info();
            CharacterWorldInfo.createModel(connection);

            CharacterInfoClass = new character_info_class();
            CharacterInfoClass.createModel(connection);
        }

        public int createNewCharacter(Character character)
        {
            string checkQuery = CharactersModel.getAllRowsByFields(new Dictionary<string, string> { { "characterName", character.CharacterName.ToLower() } }, ESelectableMethod.Equal);
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(checkQuery, connection);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    connection.Close();
                    return 0;
                }
                else
                {
                    reader.Close();

                    string query = CharactersModel.insertRow(new Dictionary<string, object>()
                    {
                        { "characterName", character.CharacterName },
                        { "characterClass", character.CharacterClass.CharacterClassCode },
                        { "accountId", character.AccountId }
                    });

                    MySqlCommand cmd1 = new MySqlCommand(query, connection);
                    cmd1.ExecuteNonQuery();

                    int newCharacterId = (int)cmd1.LastInsertedId;
                    connection.Close();

                    CreateAllInfoForCharacter(newCharacterId);

                    return newCharacterId;
                }
            }
        }

        public void CreateAllInfoForCharacter(int characterId)
        {
            int characterWorldInfoId = 0;
            int characterInfoId = 0;
            int characterDetailInfoId = 0;

            string createCharacterWorldInfo = CharacterWorldInfo.insertRow(new Dictionary<string, object>() {
                { "characterWorldInfoPosX", 330f },
                { "characterWorldInfoPosY", 16f },
                { "characterWorldInfoPosZ", 330f },
                { "characterWorldInfoLocationId", 1000 },
            });
            connection.Open();
            MySqlCommand cmd1 = new MySqlCommand(createCharacterWorldInfo, connection);
            cmd1.ExecuteNonQuery();
            characterWorldInfoId = (int)cmd1.LastInsertedId;

            string createCharacterInfo = "INSERT INTO religion.character_info VALUES();";
            MySqlCommand cmd2 = new MySqlCommand(createCharacterInfo, connection);
            cmd2.ExecuteNonQuery();
            characterInfoId = (int)cmd2.LastInsertedId;

            string createCharacterDetailInfo = "INSERT INTO religion.character_detail_info VALUES();";
            MySqlCommand cmd3 = new MySqlCommand(createCharacterDetailInfo, connection);
            cmd3.ExecuteNonQuery();
            characterDetailInfoId = (int)cmd3.LastInsertedId;

            string createCharacterInfoClass = CharacterInfoClass.insertRow(new Dictionary<string, object>() {
                { "characterId", characterId },
                { "characterInfoId", characterInfoId },
                { "characterWorldInfoId", characterWorldInfoId },
                { "characterDetailInfoId", characterDetailInfoId }
            });
            MySqlCommand cmd = new MySqlCommand(createCharacterInfoClass, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        public List<Character> getCharacterList(int accountId)
        {
            string query = CharactersModel.getAllRowsByFields(new Dictionary<string, string> { { "accountId", accountId.ToString() } });
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(query, connection);

            List<Character> characters = new List<Character>();
            foreach(characters character in CharactersModel.GetModel(cmd.ExecuteReader()))
            {
                characters.Add(new Character()
                {
                    CharacterId = character.id,
                    CharacterName = character.characterName,
                    CharacterClass = CharacterClass.CreateClassByName(character.characterClass),
                    AccountId = accountId
                });
            }
            connection.Close();

            return characters;
        }
    }
}
