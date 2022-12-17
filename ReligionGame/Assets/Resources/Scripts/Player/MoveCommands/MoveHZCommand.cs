using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MoveHZCommand : MoveCommand
{
    public MoveHZCommand(){ }

    public override void execute(PlayerMovement actor, Commands inputs)
    {
        Quaternion rotation = Quaternion.Euler(0.0f, inputs.orientation, 0.0f);

        Vector3 moveDirection = (rotation * Vector3.right * inputs.moveHorizontal + rotation * Vector3.forward * inputs.moveVertical).normalized;
        Vector3 slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, actor.slopeHit.normal);

        Vector3 move = (actor.isGrounded && actor.OnSlope()) ? slopeMoveDirection : moveDirection
            * (actor.isSprint ? actor.sprintSpeed : actor.moveSpeed)
            * (actor.isGrounded ? 1 : actor.airMultiplier);

        actor.rb.AddForce(move, ForceMode.Acceleration);

        base.execute(actor, inputs);
    }
}

