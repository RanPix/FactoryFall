using FiniteMovementStateMachine;
using UnityEngine;

public class Idle : BaseMovementState
{
    public Idle(MovementMachine stateMachine, PlayerMovement movementControl, PlayerDataFields fields)
        : base("Idle", stateMachine, movementControl, fields) { }

    #region State logic

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        ChangeVelocity();
    }

    public override void CheckForChangeState()
    {
        if (input != Vector2.zero)
            stateMachine.ChangeState(stateMachine.walk);

        if (!GetIsGrounded() || data.gotJumpInput)
            stateMachine.ChangeState(stateMachine.midAir);
    }

    #endregion

    private void ChangeVelocity()
    {
        data.horizontalMove =
            Vector2.Lerp(data.horizontalMove, Vector2.zero, fields.ScriptableFields.InterpolationRate * Time.deltaTime);
    }
}
