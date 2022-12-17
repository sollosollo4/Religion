using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

    public class JumpCommand : MoveCommand
    {
        public JumpCommand() { }
        public override void execute(PlayerMovement actor, Commands inputs)
        {
            if (actor.isGrounded)
            {
                actor.rb.velocity = new Vector3(actor.rb.velocity.x, 0, actor.rb.velocity.z);
                actor.rb.AddForce(actor.transform.up * actor.jumpForce, ForceMode.Acceleration);
            }
            base.execute(actor, inputs);
        }
    }

