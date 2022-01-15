using Assets.Database.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void createNewCharacter(Character character)
        {
            string query = CharactersModel.insertRow(new Dictionary<string, object>()
            {
                { "characterName", character.CharacterName },
                { "characterClass", character.CharacterClass.CharacterClassCode },
                { "accountId", character.AccountId }
            });
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.ExecuteReader();
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
