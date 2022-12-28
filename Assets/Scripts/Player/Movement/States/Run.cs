using FiniteMovementStateMachine;
using UnityEngine;

public class Run : BaseMovementState
{
    public Run(MovementMachine stateMachine, PlayerMovement movementControl, PlayerDataFields fields)
        : base("run", stateMachine, movementControl, fields) { }
    
    #region State logic

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        ChangeVelocity();
    }

    public override void CheckForChangeState()
    {
        if (!GetIsGrounded() || data.gotJumpInput)
            stateMachine.ChangeState(stateMachine.midAir);
        else if (!GetIsMovingForward() || !controls.Player.Sprint.IsPressed() || input == Vector2.zero)
            stateMachine.ChangeState(stateMachine.walk);
    }

    #endregion

    private void ChangeVelocity()
    {
        Vector2 desiredSpeed = fields.ScriptableFields.RunSpeed * input * fields.ScriptableFields.SpeedMultiplier;

        data.horizontalMove =
            Vector2.Lerp(data.horizontalMove, desiredSpeed, fields.ScriptableFields.InterpolationRate * Time.deltaTime);
    }
}