using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Assets.Database.Models
{
    public class accounts : Model, IDbModel
    {

        public int id { get; set; }
        public string accountLogin { get; set; }
        public string accountPassword { get; set; }
        public string accountSalt { get; set; }
        protected override string modelName  => "accounts";

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
                    accountLogin = reader.GetString(1);
                    accountPassword = reader.GetString(2);
                    accountSalt = reader.GetString(3);
                }
                
                yield return this;
            }
            reader.Close();
        }
    }
}
