using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Database.Models
{
    public class character_detail_info : Model, IDbModel
    {
        public int id { get; set; }
        protected override string modelName => "character_detail_info";

        public override void createModel(MySqlConnection context)
        {
        }

        public override IEnumerable<Model> GetModel(MySqlDataReader reader)
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    id = reader.GetInt32(0);
                }

                yield return this;
            }
            reader.Close();
        }
    }
}
