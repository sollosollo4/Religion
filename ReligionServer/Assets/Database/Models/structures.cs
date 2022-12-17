using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Database.Models
{
    public class structures : Model, IDbModel
    {
        public uint id { get; set; }
        public ushort structuresPrefabId { get; set; }
        public string structuresPrefabName { get; set; }

        public float structuresPositionX { get; set; }
        public float structuresPositionY { get; set; }
        public float structuresPositionZ { get; set; }

        public float structuresRotationX { get; set; }
        public float structuresRotationY { get; set; }
        public float structuresRotationZ { get; set; }

        protected override string modelName => "structures";

        public override void createModel(MySqlConnection context)
        {
        }

        public override IEnumerable<Model> GetModel(MySqlDataReader reader)
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    id = reader.GetUInt32(0);
                    structuresPrefabId = reader.GetUInt16(1);
                    structuresPrefabName = reader.GetString(2);

                    structuresPositionX = Convert.ToSingle(reader[3]);
                    structuresPositionY = Convert.ToSingle(reader[4]);
                    structuresPositionZ = Convert.ToSingle(reader[5]);
                                                           
                    structuresRotationX = Convert.ToSingle(reader[6]);
                    structuresRotationY = Convert.ToSingle(reader[7]);
                    structuresRotationZ = Convert.ToSingle(reader[8]);

                    yield return this;
                }
            }
            reader.Close();
        }

        public override string deleteAllRows()
        {
            string rawQeury = $"DELETE FROM {modelName} WHERE 1;";

            rawQeury += $"ALTER TABLE {modelName} AUTO_INCREMENT = 0;";

            return rawQeury;
        }
    }
}
