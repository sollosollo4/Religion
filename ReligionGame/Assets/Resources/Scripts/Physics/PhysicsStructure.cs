using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public struct Inputs
{
    public const int InputsLength = 5;
    public bool jump;
    public bool sprint;
    public Vector3 moveD;
    public Vector3 slopeD;
}

public struct InputMessage
{
    public float delivery_time;
    public float camRotation;
    public uint start_tick_number;
    public List<Inputs> inputs;
}

public struct ClientState
{
    public Vector3 position;
}

public struct StateMessage
{
    public float delivery_time;
    public uint tick_number;
    public Vector3 position;
}
