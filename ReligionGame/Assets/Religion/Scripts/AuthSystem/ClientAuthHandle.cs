using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ClientAuthHandle
{
    public static void TryConnection(Packet _packet)
    {
        bool _isConnected = _packet.ReadBool();
        string _message = _packet.ReadString();
        

        Authorization.instance.ConnectToServer(_isConnected, _message);
    }
}

