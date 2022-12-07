using FiniteMovementStateMachine;
using Unity.Mathematics;
using UnityEngine;

public class Walk : BaseMovementState
{
    public Walk(MovementMachine stateMachine, PlayerMovement movementControl)
        : base("Walk", stateMachine, movementControl) { }

    private void ChangeVelocity()
    {
        Vector2 desiredSpeed = stateMachine.fields.walkSpeed * input;

        data.horizontalMove =
            Vector2.Lerp(data.horizontalMove, desiredSpeed, stateMachine.fields.interpolationRate * Time.deltaTime);
    }

    public override void Enter(MovementDataIntersection inputData)
    {
        base.Enter(inputData);


    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        
        ChangeVelocity();

        data.CalculateHorizontalMagnitude();

        if(!isGrounded || data.gotJumpInput)
            stateMachine.ChangeState(stateMachine.midAir);

        if(!CheckIfMoving())
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
