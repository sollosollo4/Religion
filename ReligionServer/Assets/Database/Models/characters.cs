using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Database.Models
{
    public class characters : Model, IDbModel
    {
        public int id { get; set; }
        public string characterName { get; set; }
        public string characterClass { get; set; }
        public int accountId { get; set; }

        protected override string modelName => "characters";

        public character_info_class characterInfoClass;

        public override void createModel(MySqlConnection context)
        {
            /*context.Open();

            characterInfoClass = getForeignModels<character_info_class>(new Dictionary<string, string>()
            {
                { "characterId", id.ToString() }
            }, new character_info_class(), context);


            context.Close();*/
        }

        public override IEnumerable<Model> GetModel(MySqlDataReader reader)
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    id = reader.GetInt32(0);
                    characterName = reader.GetString(1);
                    characterClass = reader.GetString(2);
                    accountId = reader.GetInt32(3);

                    yield return this;
                }
            }
            reader.Close();
        }
    }
}
