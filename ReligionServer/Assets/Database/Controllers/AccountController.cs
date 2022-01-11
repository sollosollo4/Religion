using Assets.Database.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Database.Controllers
{
    public class AccountController : Controller
    {
        private MySqlConnection connection;
        accounts AccountsModel;

        public AccountController(MySqlConnection connection)
        {
            this.connection = connection;
            AccountsModel = new accounts();
            AccountsModel.createModel(connection);
        }

        public bool checkUserPasswordHash(string userName, string password)
        {
            string query = AccountsModel.getRowsByFields(new string[] {"accountPassword", "accountSalt"}, new Dictionary<string, string> { {"accountLogin", userName} });
            connection.Open();
            Debug.Log(query);
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            Debug.Log("Yes. We sure this"+ reader.HasRows);
            if (reader.HasRows)
            {
                int count = reader.FieldCount;
                string[] passwords = new string[count];
                while (reader.Read())
                {
                    for (var i = 0; i < count; i++)
                    {
                        passwords[i] = reader.GetValue(i).ToString();
                    }
                }
                reader.Close();

                return AreEqual(password, passwords[0], passwords[1]);
            }
            connection.Close();

            return false;
        }

        static string CreateSalt(int size)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        static string GenerateHash(string input, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input + salt);
            SHA256Managed sHA256ManagedString = new SHA256Managed();
            byte[] hash = sHA256ManagedString.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        static bool AreEqual(string plainTextInput, string hashedInput, string salt)
        {
            string newHashedPin = GenerateHash(plainTextInput, salt);
            return newHashedPin.Equals(hashedInput);
        }
    }
}
