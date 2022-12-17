using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Database.Models
{
    public class character_info_class : Model, IDbModel
    {
        public int id { get; set; }
        public int characterId { get; set; }
        public int characterInfoId { get; set; }
        public int characterWorldInfoId { get; set; }
        public int characterDetailInfoId { get; set; }

        protected override string modelName => "character_info_class";

        public character_world_info characterWorldInfo;
        public character_info characterInfo;
        public character_detail_info characterDetailInfo;

        public override void createModel(MySqlConnection context)
        {
            /*context.Open();

            characterWorldInfo = getForeignModels<character_world_info>(new Dictionary<string, string>()
            {
                { "id", characterWorldInfoId.ToString() }
            }, new character_world_info(), context);

            characterInfo = getForeignModels<character_info>(new Dictionary<string, string>()
            {
                { "id", characterInfoId.ToString() }
            }, new character_info(), context);

            characterDetailInfo = getForeignModels<character_detail_info>(new Dictionary<string, string>()
            {
                { "id", characterDetailInfoId.ToString() }
            }, new character_detail_info(), context);

            context.Close();*/
        }

        public override IEnumerable<Model> GetModel(MySqlDataReader reader)
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    id = reader.GetInt32(0);
                    characterId = reader.GetInt32(1);
                    characterInfoId = reader.GetInt32(2);
                    characterWorldInfoId = reader.GetInt32(3);
                    characterDetailInfoId = reader.GetInt32(4);

                    yield return this;
                }
            }
            reader.Close();
        }
    }
}
