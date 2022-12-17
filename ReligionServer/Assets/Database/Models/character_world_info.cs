using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Database.Models
{
    public class character_world_info : Model, IDbModel
    {
        public int id { get; set; }
        public float characterWorldInfoPosX { get; set; }
        public float characterWorldInfoPosY { get; set; }
        public float characterWorldInfoPosZ { get; set; }
        public int characterWorldInfoLocationId { get; set; }
        protected override string modelName => "character_world_info";

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
                    characterWorldInfoPosX = reader.GetFloat(1);
                    characterWorldInfoPosY = reader.GetFloat(2);
                    characterWorldInfoPosZ = reader.GetFloat(3);
                    characterWorldInfoLocationId = reader.GetInt32(4);
                }

                yield return this;
            }
            reader.Close();
        }
    }
}