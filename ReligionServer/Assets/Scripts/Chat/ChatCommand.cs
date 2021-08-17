using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public class ChatCommand : ServerSend
{
    public static readonly string[] ChatCommands = { ".set_time_night", ".set_time_day" };
    
    public enum ChatPackets 
    {
        forbidden = 1,
        setTimeNight,
        setTimeDay
    }

    public delegate void CommandHandler(params string[] _params);
    public static Dictionary<string, CommandHandler> executer = new Dictionary<string, CommandHandler>();

    /****
     * .set_time_night - simple command. set night time at server.
     * .set_new_spawn_enemy_point 8  - medium command. spawn point of enemies with level 8
     * .give_item 0,10,0  - hard command. give item to player with id 0, quantity 10, and level 0
     * .set_new_spawn_enemy_point 8,[4.55,78.11,19.1] - very hard command. spawn point of enemies with level 8, at coordinations x:4.55, y:78.11, z:19.1
     */
    public ChatCommand()
    {
        executer.Add("ForbiddenError", ForbiddenError);
        executer.Add(".set_time_night", SetTimeNight);
        executer.Add(".set_time_day", SetTimeDay);
    }

    public void ExecuteCommand(string _name, string[] _params)
    {
        executer[_name].Invoke(_params);
    }

    #region Packets
    public static void SetTimeNight(string[] _params)
    {
        using (Packet _packet = new Packet((int)ChatPackets.setTimeNight))
        {
            _packet.Write("set_time_night");
            _packet.Write("night");
            foreach(string param in _params)
                _packet.Write(param);

            SendUDPDataToAll(_packet);
        }
    }

    public static void SetTimeDay(string[] _params)
    {
        using (Packet _packet = new Packet((int)ChatPackets.setTimeDay))
        {
            _packet.Write("set_time_day");
            _packet.Write("day");
            foreach (string param in _params)
                _packet.Write(param);

            SendUDPDataToAll(_packet);
        }
    }

    public static void ForbiddenError(string [] _params)
    {
        using (Packet _packet = new Packet((int)ChatPackets.forbidden))
        {
            _packet.Write("forbidden");
            _packet.Write("I ask you not to express yourself on my server!");
            foreach (string param in _params)
                _packet.Write(param);

            SendUDPDataToAll(_packet);
        }
    }
    #endregion
}

