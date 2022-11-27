using FiniteMovementStateMachine;
using UnityEngine;

public class Idle : BaseMovementState
{
    public Idle(MovementMachine stateMachine) : base("Idle", stateMachine) { }

    public override void Enter(MovementDataIntersection data)
    {
        base.Enter(data);

        data.horizontalMove = Vector2.zero;
        data.verticalMove = 0f;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

    }

    public override void UpdatePhysics()
    {

    }

    public override MovementDataIntersection Exit()
    {

        return base.Exit();
    }
}
