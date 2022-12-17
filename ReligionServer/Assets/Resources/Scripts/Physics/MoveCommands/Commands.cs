using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct Commands
{
    public bool jump;
    public bool sprint;

    public sbyte moveHorizontal;
    public sbyte moveVertical;

    public short orientation; // camera 360 (actually more)
}

public struct CommandMessage
{
    public uint start_tick_number;
    public List<Commands> inputs;
}
