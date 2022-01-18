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

        public override void createModel(MySqlConnection context)
        {

        }

        public IEnumerable<Model> GetModel(MySqlDataReader reader)
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
