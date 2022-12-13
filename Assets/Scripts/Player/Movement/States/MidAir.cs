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

        Vector2 addition = input * stateMachine.fields.airSpeed * Time.deltaTime;
        Vector2 desiredSpeed = data.horizontalMove + addition;

        data.CalculateHorizontalMagnitude();

        if (desiredSpeed.magnitude > stateMachine.fields.maxAirSpeed)
            desiredSpeed = desiredSpeed.normalized * stateMachine.fields.maxAirSpeed;

        data.horizontalMove = desiredSpeed; 

        //Vector2.Lerp(data.horizontalMove, desiredSpeed, stateMachine.fields.interpolationRate * Time.deltaTime);
    }

    #region State logic

    public override void Enter(MovementDataIntersection inputData)
    {
        base.Enter(inputData);

        hasDoubleJumps = stateMachine.fields.doubleJumps;
        hasRedirects = stateMachine.fields.redirects;

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

        if(isGrounded && data.verticalMove < math.EPSILON)
        {
            if(CheckIfMoving())
                stateMachine.ChangeState(stateMachine.walk);
            else
                stateMachine.ChangeState(stateMachine.idle);
        }
    }

    public override MovementDataIntersection Exit()
    {
        data.verticalMove = 0f;
        return base.Exit();
    }

    #endregion

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

    private void AddRedirect(InputAction.CallbackContext context)
        => gotRedirect = true;

    private bool TryRedirect()
    {
        gotRedirect = false;

        if (!CheckForCharges())
            return false;

        data.CalculateHorizontalMagnitude();

        data.horizontalMove = data.horizontalMagnitude * input;

        return true;
    }

    private bool CheckForCharges()
    {
        if(!data.IsMovingHorizontally())
            return false;

        return hasRedirects-- > 0;
    }
}