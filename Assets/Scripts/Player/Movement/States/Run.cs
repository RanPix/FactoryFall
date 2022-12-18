using FiniteMovementStateMachine;
using UnityEngine;

public class Run : BaseMovementState
{
    public Run(MovementMachine stateMachine, PlayerMovement movementControl)
        : base("run", stateMachine, movementControl) { }

    private void ChangeVelocity()
    {
        Vector2 desiredSpeed = stateMachine.fields.RunSpeed * input * stateMachine.fields.SpeedMultiplier;

        data.horizontalMove =
            Vector2.Lerp(data.horizontalMove, desiredSpeed, stateMachine.fields.InterpolationRate * Time.deltaTime);
    }

    #region State logic

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        ChangeVelocity();

        CheckForChangeState();
    }

    protected override void CheckForChangeState()
    {
        if (!isGrounded || data.gotJumpInput)
            stateMachine.ChangeState(stateMachine.midAir);
        else if (!isMovingForward || !controls.Player.Sprint.IsPressed())
            stateMachine.ChangeState(stateMachine.walk);
    }

    #endregion

}