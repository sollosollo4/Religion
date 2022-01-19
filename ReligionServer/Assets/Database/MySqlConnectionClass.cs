using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Assets.Database.Exceptions;
using UnityEngine;
using MySql.Data.MySqlClient;
using Assets.Database.Controllers;

namespace Assets.Database
{
    public class MySqlConnectionClass
    {
        MySqlConnection connection;

        public MySqlConnection getConnection() => connection;

        Dictionary<string, Controller> Controllers;

        public MySqlConnectionClass()
        {
            connection = new MySqlConnection("Server=localhost;Database=religion;port=3306;User Id=religion;password=religion");
            connection.Open();
            Debug.Log($"Connect to MySql server. Server version: {connection.ServerVersion}");

            MySqlCommand command = new MySqlCommand("INSERT server_logs(logText) VALUES('Successfull contected from server')", connection);
            command.ExecuteReader();
            connection.Close();

            setupControllers();
        }

        private void setupControllers()
        {
            Controllers = new Dictionary<string, Controller>
            {
                { "AccountController", new AccountController(connection) },
                { "CharacterController", new Controllers.CharacterController(connection) },
                { "StructureController", new StructureController(connection) },
            };
        }

        public void MySqlCloseConnection()
        {
            connection.Open();
            MySqlCommand command = new MySqlCommand("INSERT server_logs(logText) VALUES('The server has been shut down. Termination of connection')", connection);
            command.ExecuteReader();

            connection.Close();
        }

        public Controller getController<Controller>()
        {
            Type typeParamtrType = typeof(Controller);
            if(Controllers.ContainsKey(typeParamtrType.Name))
                return (Controller)Controllers[typeParamtrType.Name];
            else
            {
                throw new Exception("MYSQL: Error get controller with name: "+ typeParamtrType.Name);
            }
        }
    }
}
