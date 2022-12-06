using System;
using FiniteMovementStateMachine;
using Unity.Mathematics;
using UnityEngine;

public class Walk : BaseMovementState
{
    public Walk(MovementMachine stateMachine, PlayerMovement movementControl)
        : base("Walk", stateMachine, movementControl) { }

    private void ChangeVelocity()
    {
        Vector2 desiredSpeed = DataFields.walkSpeed * input;

        data.horizontalMove =
            Vector2.Lerp(data.horizontalMove, desiredSpeed, DataFields.interpolationRate * Time.deltaTime);
    }

    public override void Enter(MovementDataIntersection inputData)
    {
        base.Enter(inputData);


    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        ChangeVelocity();

        if(data.horizontalMagnitude < math.EPSILON)
            stateMachine.ChangeState(stateMachine.idle);
        
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }

    public override MovementDataIntersection Exit()
    {

        return base.Exit();
    }
}
