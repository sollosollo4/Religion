using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MoveCommand
{
    public MoveCommand() { }
    public virtual void execute(PlayerPhysics actor, Commands command) {}
}