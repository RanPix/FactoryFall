using FiniteMovementStateMachine;
using UnityEngine;

public class Walk : BaseMovementState
{
    public Walk(MovementMachine stateMachine, PlayerMovement movementControl)
        : base("Walk", stateMachine, movementControl) { }

    private void ChangeVelocity()
    {
        Vector2 desiredSpeed = stateMachine.fields.walkSpeed * input * stateMachine.fields.speedMultiplier;

        data.horizontalMove =
            Vector2.Lerp(data.horizontalMove, desiredSpeed, stateMachine.fields.interpolationRate * Time.deltaTime);
    }

    #region State logic
    
    public override void UpdateLogic()
    {
        base.UpdateLogic();

        ChangeVelocity();

        data.CalculateHorizontalMagnitude();

        if (!isGrounded || data.gotJumpInput)
            stateMachine.ChangeState(stateMachine.midAir);
        else if (!CheckIfMoving())
            stateMachine.ChangeState(stateMachine.idle);
        else if (isMovingForward && controls.Player.Sprint.IsPressed())
            stateMachine.ChangeState(stateMachine.run);

    }
    
    #endregion
}
