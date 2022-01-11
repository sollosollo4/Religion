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
    public static bool CheckUserPasswordhash(string _username, string _password, int _fromClient)
    {
        if (Server.mySqlConnection.getController<AccountController>().checkUserPasswordHash(_username, _password))
        {
            Server.clients[_fromClient].accountId = Server.mySqlConnection.getController<AccountController>().getAccountId(_username);
            return true;
        }
        else
        {
            return false;
        }
    }
}

