using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public struct Inputs
{
    public bool jump;
    public bool sprint;

    public sbyte moveHorizontal;
    public sbyte moveVertical;

    public short orientation;
}

public struct InputMessage
{
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
