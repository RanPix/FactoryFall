using FiniteMovementStateMachine;
using Unity.Mathematics;
using UnityEngine;

public class MidAir : BaseMovementState
{
    private int hasDoubleJumps;

    public MidAir(MovementMachine stateMachine, PlayerMovement movementControl)
        : base("MidAir", stateMachine, movementControl) { }

    public override void Enter(MovementDataIntersection inputData)
    {
        base.Enter(inputData);

        hasDoubleJumps = stateMachine.fields.doubleJumps;
    }

    

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        ApplyGravity();

        if(data.gotJumpInput)
            TryJump();

        if(isGrounded && data.verticalMove < math.EPSILON)
        {
            if(CheckIfMoving())
                stateMachine.ChangeState(stateMachine.walk);
            else
                stateMachine.ChangeState(stateMachine.idle);
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }

    public override MovementDataIntersection Exit()
    {
        data.verticalMove = 0f;
        return base.Exit();
    }

    private void ApplyGravity()
        => data.verticalMove -= stateMachine.fields.gravity * Time.deltaTime;

    private void TryJump()
    {
        if (isGrounded)
            data.verticalMove += stateMachine.fields.jumpHeight;
        else if (hasDoubleJumps-- > 0)
            DoubleJump();

        data.gotJumpInput = false;
    }

    private void DoubleJump()
    {
        if (stateMachine.fields.jumpOverlap)
            data.verticalMove =
                data.verticalMove < stateMachine.fields.jumpHeight ? 
                    stateMachine.fields.jumpHeight : 
                    data.verticalMove + stateMachine.fields.jumpHeight;
        else
            data.verticalMove += stateMachine.fields.jumpHeight;
    }
}