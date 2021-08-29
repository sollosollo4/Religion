using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Character
{
    public string CharacterName;
    public string CharacterClassName;
    public string CharacterLastLocationName;

    public void WriteData(Packet packet)
    {
        packet.Write(CharacterName);
        packet.Write(CharacterClassName);
        packet.Write(CharacterLastLocationName);
    }
}
