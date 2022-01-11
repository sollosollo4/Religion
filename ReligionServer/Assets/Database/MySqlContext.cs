
using Assets.Database.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Assets.Database
{
    public class MySqlContext
    {
        MySqlConnectionClass MySqlConnection;
        public MySqlContext(MySqlConnectionClass _connection)
        {
            MySqlConnection = _connection;
        }

        List<accounts> Accounts { get; set; }
    }
}