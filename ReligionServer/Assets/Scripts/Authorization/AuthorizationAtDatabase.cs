using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Security.Cryptography;
using Assets.Database;
using Assets.Database.Controllers;

public class AuthorizationAtDatabase
{
    public static bool CheckUserPasswordhash(string _username, string _password)
    {
        return Server.mySqlConnection.getController<AccountController>().checkUserPasswordHash(_username, _password);
    }
}

