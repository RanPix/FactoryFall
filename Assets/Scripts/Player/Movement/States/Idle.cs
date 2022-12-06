using FiniteMovementStateMachine;
using UnityEngine;

public class Idle : BaseMovementState
{
    private bool jumpInput;


    public Idle(MovementMachine stateMachine, PlayerMovement movementControl)
        : base("Idle", stateMachine, movementControl) { }

    public override void Enter(MovementDataIntersection inputData)
    {
        base.Enter(inputData);

        data.horizontalMove = Vector2.zero;
        data.verticalMove = 0f;

        jumpInput = false;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if(input != Vector2.zero)
            stateMachine.ChangeState(stateMachine.walk);

        if(jumpInput || isGrounded)
            stateMachine.ChangeState(stateMachine.midAir);
    }

    public override void UpdatePhysics()
    {

    }

    public override MovementDataIntersection Exit()
    {

        return base.Exit();
    }
}
