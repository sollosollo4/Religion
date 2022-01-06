using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Database;
using System.Text;
using System.Security.Cryptography;

public class AuthorizationAtDatabase
{
    public static bool CheckUserPasswordhash(string _username, string _password)
    {
        ReligionDbContainer religionDb = new ReligionDbContainer();
        if(religionDb.UserDatas.First(x => x.Username == _username) is var user && user != null)
        {
            ConfirmPassword(_password, user.PasswordHash, user.PasswordSalt);
            return true;
        }
        
        return false;
    }

    public static bool ConfirmPassword(string password, byte[] _passwordHash, byte[] _passwordSalt)
    {
        byte[] passwordHash = Hash(password, _passwordSalt);

        return _passwordHash.SequenceEqual(passwordHash);
    }


    public static byte[] Hash(string value, byte[] salt)
    {
        return Hash(Encoding.UTF8.GetBytes(value), salt);
    }

    public static byte[] Hash(byte[] value, byte[] salt)
    {
        byte[] saltedValue = value.Concat(salt).ToArray();
        // Alternatively use CopyTo.
        //var saltedValue = new byte[value.Length + salt.Length];
        //value.CopyTo(saltedValue, 0);
        //salt.CopyTo(saltedValue, value.Length);

        return new SHA256Managed().ComputeHash(saltedValue);
    }
}

