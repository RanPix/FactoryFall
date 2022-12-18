using FiniteMovementStateMachine;
using UnityEngine;

public class Walk : BaseMovementState
{
    public Walk(MovementMachine stateMachine, PlayerMovement movementControl)
        : base("Walk", stateMachine, movementControl) { }

    private void ChangeVelocity()
    {
        Vector2 desiredSpeed = stateMachine.fields.WalkSpeed * input * stateMachine.fields.SpeedMultiplier;

        data.horizontalMove =
            Vector2.Lerp(data.horizontalMove, desiredSpeed, stateMachine.fields.InterpolationRate * Time.deltaTime);
    }

    #region State logic
    
    public override void UpdateLogic()
    {
        base.UpdateLogic();

        ChangeVelocity();

        data.CalculateHorizontalMagnitude();

        CheckForChangeState();
    }

    protected override void CheckForChangeState()
    {
        if (!isGrounded || data.gotJumpInput)
            stateMachine.ChangeState(stateMachine.midAir);
        else if (!CheckIfHaveInput())
            stateMachine.ChangeState(stateMachine.idle);
        else if (isMovingForward && controls.Player.Sprint.IsPressed())
            stateMachine.ChangeState(stateMachine.run);
    }

    #endregion
}
