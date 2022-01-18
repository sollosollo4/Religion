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
        public IDbModel DbModel => CharactersModel;

        public CharacterController(MySqlConnection connection)
        {
            this.connection = connection;
            CharactersModel = new characters();
            CharactersModel.createModel(connection);
        }

        public bool createNewCharacter(Character character)
        {
            string checkQuery = CharactersModel.getAllRowsByFields(new Dictionary<string, string> { { "characterName", character.CharacterName.ToLower() } }, ESelectableMethod.Equal);
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(checkQuery, connection);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    connection.Close();
                    return false;
                }
                else
                {
                    reader.Close();

                    Debug.Log($"Character: [{character.AccountId}] {character.CharacterName} {character.CharacterClass.CharacterClassCode}");
                    string query = CharactersModel.insertRow(new Dictionary<string, object>()
                    {
                        { "characterName", character.CharacterName },
                        { "characterClass", character.CharacterClass.CharacterClassCode },
                        { "accountId", character.AccountId }
                    });

                    MySqlCommand cmd1 = new MySqlCommand(query, connection);
                    cmd1.ExecuteReader();
                    connection.Close();

                    return true;
                }
            }
        }

        public List<Character> getCharacterList(int accountId)
        {
            string query = CharactersModel.getAllRowsByFields(new Dictionary<string, string> { { "accountId", accountId.ToString() } });
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(query, connection);

            List<Character> characters = new List<Character>();
            foreach(characters character in CharactersModel.GetModel(cmd.ExecuteReader()))
            {
                Debug.Log($"Character {character.accountId} {character.characterName} {character.characterClass}");
                characters.Add(new Character()
                {
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
