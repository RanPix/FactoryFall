using FiniteMovementStateMachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class MidAir : BaseMovementState
{
    private int hasDoubleJumps;
    private int hasRedirects;

    private bool gotRedirect;

    public MidAir(MovementMachine stateMachine, PlayerMovement movementControl, PlayerDataFields fields)
        : base("MidAir", stateMachine, movementControl, fields)
    {
        controls.Player.Redirect.performed += AddRedirect;
    }

    #region State logic

    public override void Enter(MovementDataIntersection inputData)
    {
        base.Enter(inputData);

        hasDoubleJumps = fields.ScriptableFields.DoubleJumps;
        hasRedirects = fields.ScriptableFields.Redirects;

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
    }

    public override void CheckForChangeState()
    {
        if (GetIsGrounded() && data.verticalMove < math.EPSILON)
        {
            if (input != Vector2.zero)
                stateMachine.ChangeState(stateMachine.walk);
            else
                stateMachine.ChangeState(stateMachine.idle);
        }
        else if(GetGotWall())
            stateMachine.ChangeState(stateMachine.wallrun);
    }

    public override MovementDataIntersection Exit()
    {
        data.verticalMove = 0f;
        data.lastWallNormal = Vector3.zero;

        return base.Exit();
    }

    #endregion

    private void ChangeVelocity()
    {
        Vector2 changedInput = input;
        changedInput.x *= 0.5f;
        changedInput = changedInput.normalized;

        Vector2 addition = input * fields.ScriptableFields.AirSpeed * Time.deltaTime;
        Vector2 desiredSpeed = data.horizontalMove + addition;

        if (desiredSpeed.magnitude > fields.ScriptableFields.MaxAirSpeed)
        {
            data.horizontalMove = desiredSpeed.normalized * data.horizontalMove.magnitude;
        }
        else
            data.horizontalMove = desiredSpeed;
    }

    private void ApplyGravity()
    {
        if(data.verticalMove > -fields.ScriptableFields.MaxFallSpeed)
            data.verticalMove -= fields.ScriptableFields.Gravity * Time.deltaTime;
    }

    private void TryJump()
    {
        if (GetIsGrounded())
            data.verticalMove += fields.ScriptableFields.JumpHeight;
        else if (hasDoubleJumps-- > 0)
            DoubleJump();

        data.gotJumpInput = false;
    }

    private void DoubleJump()
    {
        if (fields.ScriptableFields.JumpOverlap)
            data.verticalMove =
                data.verticalMove < fields.ScriptableFields.JumpHeight ?
                    fields.ScriptableFields.JumpHeight : 
                    data.verticalMove + fields.ScriptableFields.JumpHeight;
        else
            data.verticalMove += fields.ScriptableFields.JumpHeight;
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