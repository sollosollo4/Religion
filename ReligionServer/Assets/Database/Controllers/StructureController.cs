using Assets.Database.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Database.Controllers
{
    public class StructureController : Controller
    {
        private MySqlConnection connection;
        structures StructuresModel;
        public IDbModel DbModel => StructuresModel;

        public StructureController(MySqlConnection connection)
        {
            this.connection = connection;
            StructuresModel = new structures();
            StructuresModel.createModel(connection);
        }

        public Dictionary<uint, SpawnedGameObject> loadPrefabs()
        {
            Dictionary<uint, SpawnedGameObject> getPrefabs = new Dictionary<uint, SpawnedGameObject>();

            string getQuery = StructuresModel.getAllRows();
            connection.Open();
            MySqlCommand com = new MySqlCommand(getQuery, connection);
            using (MySqlDataReader reader = com.ExecuteReader())
            {
                foreach (structures model in StructuresModel.GetModel(reader))
                {
                    var prefab = Resources.Load<GameObject>($"SpawnablePrefabs/{model.structuresPrefabName}");
                    SpawnedGameObject spawnedGameObject = prefab.GetComponent<SpawnedGameObject>();
                    getPrefabs.Add(model.id, spawnedGameObject);
                }
            }
            connection.Close();
            return getPrefabs;
        }

        public void savePrefabs(SpawnedGameObject[] touchableStructures)
        {
            string checkQuery = StructuresModel.deleteAllRows();
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(checkQuery, connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Close();

            foreach (SpawnedGameObject touch in touchableStructures)
            {
                string query = StructuresModel.insertRow(new Dictionary<string, object> 
                {
                    { "id", touch.spawnedObjectId.ToString("G", CultureInfo.InvariantCulture) },
                    { "structuresPrefabId", touch.PrefabId.ToString("G", CultureInfo.InvariantCulture) },
                    { "structuresPrefabName", touch.name },

                    { "structuresPositionX", touch.gameObject.transform.position.x.ToString("G", CultureInfo.InvariantCulture) },
                    { "structuresPositionY", touch.gameObject.transform.position.y.ToString("G", CultureInfo.InvariantCulture) },
                    { "structuresPositionZ", touch.gameObject.transform.position.z.ToString("G", CultureInfo.InvariantCulture) },

                    { "structuresRotationX", touch.gameObject.transform.position.x.ToString("G", CultureInfo.InvariantCulture) },
                    { "structuresRotationY", touch.gameObject.transform.position.y.ToString("G", CultureInfo.InvariantCulture) },
                    { "structuresRotationZ", touch.gameObject.transform.position.z.ToString("G", CultureInfo.InvariantCulture) },
                });
                Debug.Log(query);

                MySqlCommand addCmd = new MySqlCommand(query, connection);
                MySqlDataReader addReader = addCmd.ExecuteReader();
                addReader.Close();
            }

            connection.Close();
        }
    }
}
