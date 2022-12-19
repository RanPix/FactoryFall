using FiniteMovementStateMachine;
using UnityEngine;

public class Walk : BaseMovementState
{
    public Walk(MovementMachine stateMachine, PlayerMovement movementControl, PlayerDataFields fields)
        : base("Walk", stateMachine, movementControl, fields) { }

    #region State logic
    
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        CheckIfMovingForward();

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

    private void ChangeVelocity()
    {
        Vector2 desiredSpeed = fields.ScriptableFields.WalkSpeed * input * fields.ScriptableFields.SpeedMultiplier;

        data.horizontalMove =
            Vector2.Lerp(data.horizontalMove, desiredSpeed, fields.ScriptableFields.InterpolationRate * Time.deltaTime);
    }
}
