using FiniteMovementStateMachine;
using UnityEngine;

public class Walk : BaseMovementState
{
    public Walk(MovementMachine stateMachine, PlayerMovement movementControl, PlayerDataFields fields, MovementDataIntersection data, PlayerControls controls)
        : base("Walk", stateMachine, movementControl, fields, data, controls) { }

    #region State logic
    
    public override void UpdateLogic()
    {
        base.UpdateLogic();

        ChangeVelocity();

        data.CalculateHorizontalMagnitude();
    }

    public override void CheckForChangeState()
    {
        if (!GetIsGrounded() || data.gotJumpInput)
            stateMachine.ChangeState(stateMachine.midAir);
        else if (input == Vector2.zero)
            stateMachine.ChangeState(stateMachine.idle);
        else if (GetIsMovingForward() && controls.Player.Sprint.IsPressed())
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
