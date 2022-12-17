using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Assets.Database.Exceptions;
using MySql.Data.MySqlClient;
using Assets.Database.Controllers;

namespace Assets.Database
{
    public class MySqlConnectionSingletone
    {
        MySqlConnection connection;

        public MySqlConnection getConnection() => connection;

        Dictionary<string, Controller> Controllers;

        public MySqlConnectionSingletone()
        {
            connection = new MySqlConnection("Server=localhost;Database=religion;port=3306;User Id=religion;password=religion");
            try
            {
                UnityEngine.Debug.Log($"Try Connect to MySql server.");
                connection.Open();
                UnityEngine.Debug.Log($"MySql version: { connection.ServerVersion}");

                MySqlCommand command = new MySqlCommand("INSERT server_logs(logText) VALUES('Successfull contected from server')", connection);
                command.ExecuteReader();
                connection.Close();
                UnityEngine.Debug.Log($"Connecting to MySql successfully!");
            }
            catch(Exception e)
            {
                UnityEngine.Debug.LogError($"Error connection to MySql server: {e.Message}");
                UnityEngine.Debug.LogException(e);
            }
        }

        public void setupControllers()
        {
            Controllers = new Dictionary<string, Controller>
            {
                { "AccountController", new AccountController(connection) },
                { "CharacterController", new CharacterController(connection) },
                { "StructureController", new StructureController(connection) },
                { "CharacterInfoClassController", new CharacterInfoClassController(connection) },
                { "CharacterWorldInfoController", new CharacterWorldInfoController(connection) },
                //{ "CharacterDetailInfoController", new CharacterDetailInfoController(connection) }
            };
        }

        public void MySqlConnectionClose()
        {
            connection.Open();
            MySqlCommand command = new MySqlCommand("INSERT server_logs(logText) VALUES('The server has been shut down. Termination of connection')", connection);
            command.ExecuteReader();

            connection.Close();
        }

        public Controller getController<Controller>()
        {
            Type typeParamtrType = typeof(Controller);
            if (Controllers.ContainsKey(typeParamtrType.Name))
            {
                return (Controller)Controllers[typeParamtrType.Name];
            }
            else
            {
                UnityEngine.Debug.LogError("MYSQL: Error get controller with name: " + typeParamtrType.Name);
                throw new MySqlExceptions("MYSQL: Error get controller with name: " + typeParamtrType.Name);
            }
        }
    }
}
