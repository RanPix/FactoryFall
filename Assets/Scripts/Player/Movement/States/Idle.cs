using FiniteMovementStateMachine;
using UnityEngine;

public class Idle : BaseMovementState
{
    public Idle(MovementMachine stateMachine, PlayerMovement movementControl)
        : base("Idle", stateMachine, movementControl) { }

    #region State logic

    public override void Enter(MovementDataIntersection inputData)
    {
        base.Enter(inputData);

        data.horizontalMove = Vector2.zero;
        data.verticalMove = 0f;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        CheckForChangeState();
    }

    protected override void CheckForChangeState()
    {
        if (input != Vector2.zero)
            stateMachine.ChangeState(stateMachine.walk);

        if (!isGrounded || data.gotJumpInput)
            stateMachine.ChangeState(stateMachine.midAir);
    }

    #endregion
}
