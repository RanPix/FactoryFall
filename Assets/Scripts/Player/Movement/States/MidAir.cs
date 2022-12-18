using FiniteMovementStateMachine;
using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class MidAir : BaseMovementState
{
    private int hasDoubleJumps;
    private int hasRedirects;

    private bool gotRedirect;

    public MidAir(MovementMachine stateMachine, PlayerMovement movementControl)
        : base("MidAir", stateMachine, movementControl)
    {
        controls.Player.Redirect.performed += AddRedirect;
    }

    private void ChangeVelocity()
    {
        Vector2 changedInput = input;
        changedInput.x *= 0.5f;
        changedInput = changedInput.normalized;

        Vector2 addition = input * stateMachine.fields.AirSpeed * Time.deltaTime;
        Vector2 desiredSpeed = data.horizontalMove + addition;

        data.CalculateHorizontalMagnitude();

        if (desiredSpeed.magnitude > stateMachine.fields.MaxAirSpeed)
            desiredSpeed = desiredSpeed.normalized * stateMachine.fields.MaxAirSpeed;

        data.horizontalMove = desiredSpeed; 

        //Vector2.Lerp(data.horizontalMove, desiredSpeed, stateMachine.fields.interpolationRate * Time.deltaTime);
    }

    #region State logic

    public override void Enter(MovementDataIntersection inputData)
    {
        base.Enter(inputData);

        hasDoubleJumps = stateMachine.fields.DoubleJumps;
        hasRedirects = stateMachine.fields.Redirects;

        gotRedirect = false;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        ChangeVelocity();

        data.CalculateHorizontalMagnitude();

        ApplyGravity();

        if(data.gotJumpInput)
            TryJump();

        if(gotRedirect)
            TryRedirect();

        CheckForChangeState();
    }

    protected override void CheckForChangeState()
    {
        if (isGrounded && data.verticalMove < math.EPSILON)
        {
            if (CheckIfHaveInput())
                stateMachine.ChangeState(stateMachine.walk);
            else
                stateMachine.ChangeState(stateMachine.idle);
        }
        else if(gotWall)
            stateMachine.ChangeState(stateMachine.wallrun);
    }

    public override MovementDataIntersection Exit()
    {
        data.verticalMove = 0f;
        return base.Exit();
    }

    #endregion

    private void ApplyGravity()
        => data.verticalMove -= stateMachine.fields.Gravity * Time.deltaTime;

    private void TryJump()
    {
        if (isGrounded)
            data.verticalMove += stateMachine.fields.JumpHeight;
        else if (hasDoubleJumps-- > 0)
            DoubleJump();

        data.gotJumpInput = false;
    }

    private void DoubleJump()
    {
        if (stateMachine.fields.JumpOverlap)
            data.verticalMove =
                data.verticalMove < stateMachine.fields.JumpHeight ? 
                    stateMachine.fields.JumpHeight : 
                    data.verticalMove + stateMachine.fields.JumpHeight;
        else
            data.verticalMove += stateMachine.fields.JumpHeight;
    }

    private void AddRedirect(InputAction.CallbackContext context)
        => gotRedirect = true;

    private void TryRedirect()
    {
        gotRedirect = false;

        if (!CheckForCharges()) 
            return;

        data.CalculateHorizontalMagnitude();

        data.horizontalMove = data.horizontalMagnitude * input;
    }

    private bool CheckForCharges()
    {
        if(!data.IsMovingHorizontally())
            return false;

        return hasRedirects-- > 0;
    }
}