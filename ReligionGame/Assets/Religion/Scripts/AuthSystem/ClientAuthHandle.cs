using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ClientAuthHandle
{
    public static void TryConnection(Packet _packet)
    {
        bool _isConnected = _packet.ReadBool();
        string _message = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Client.instance.myId = _myId;

        // Now that we have the client's id, connect UDP
        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);

        Authorization.instance.ConnectToServer(_isConnected, _message);
    }
}

