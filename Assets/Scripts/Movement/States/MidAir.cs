using System;
using FiniteMovementStateMachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class MidAir : BaseMovementState
{
    public Action<Vector2> OnRedirect;

    public int hasDoubleJumps
    {
        get => _hasDoubleJumps;
        private set
        {
            _hasDoubleJumps = value;
            OnDoubleJumpsCountChange?.Invoke(value);
        }
    }
    private int _hasDoubleJumps;
    public Action<int> OnDoubleJumpsCountChange;

    public int hasRedirects
    {
        get => _hasRedirects;
        private set
        {
            _hasRedirects = value;
            OnRedirectsCountChange?.Invoke(value);
        }
    }
    private int _hasRedirects;
    public Action<int> OnRedirectsCountChange;

    private AudioSync audioSync;
    private bool gotRedirectInput;

    public MidAir(MovementMachine stateMachine, PlayerMovement movementControl, PlayerDataFields fields, MovementDataIntersection data, PlayerControls controls)
        : base("MidAir", stateMachine, movementControl, fields, data, controls)
    {
        controls.Player.Redirect.performed += AddRedirect;
    }



    private void Awake()
    {
        audioSync = NetworkClient.localPlayer.gameObject.GetComponent<AudioSync>();
    }

    #region State logic

    public override void Enter()
    {
        base.Enter();

        hasDoubleJumps = fields.ScriptableFields.DoubleJumps;
        hasRedirects = fields.ScriptableFields.Redirects;

        gotRedirectInput = false;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        ChangeVelocity();

        data.CalculateHorizontalMagnitude();

        ApplyGravity();

        if(data.gotJumpInput)
            Jump();

        if(gotRedirectInput)
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

    public override void Exit()
    {
        data.verticalMove = 0f;
        data.lastWallNormal = Vector3.zero;

        base.Exit();
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

    private void Jump()
    {
        if (fields.ScriptableFields.JumpOverlap) // Jump overlap fix check
            data.verticalMove =
                data.verticalMove < fields.ScriptableFields.JumpHeight ?
                    fields.ScriptableFields.JumpHeight : 
                    data.verticalMove + fields.ScriptableFields.JumpHeight;
        else
            data.verticalMove += fields.ScriptableFields.JumpHeight;
        if (!audioSync)
            audioSync = NetworkClient.localPlayer.gameObject.GetComponent<AudioSync>();
        audioSync?.PlaySound(ClipType.player, true, "Jump");
        data.gotJumpInput = false;
    }

    private void AddRedirect(InputAction.CallbackContext context)
    {
        if(isCurrentState)
            gotRedirectInput = true;
    }

    protected override void AddJump(InputAction.CallbackContext context)
    {
        if(!isCurrentState)
            return;

        if (hasDoubleJumps-- > 0)
            base.AddJump(context);
    }

    private void TryRedirect()
    {
        gotRedirectInput = false;

        if (!CheckForCharges()) 
            return;
        if (!audioSync)
            audioSync = NetworkClient.localPlayer.gameObject.GetComponent<AudioSync>();

        audioSync?.PlaySound(ClipType.player, true, "Redirect");
        data.CalculateHorizontalMagnitude();

        data.horizontalMove = data.horizontalMagnitude * input;

        OnRedirect.Invoke(input);
    }

    private bool CheckForCharges()
    {
        if(!data.IsMovingHorizontally())
            return false;

        return hasRedirects-- > 0;
    }
}