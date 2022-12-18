using FiniteMovementStateMachine;
using UnityEngine;

public class Wallrun : BaseMovementState
{
    public Wallrun(MovementMachine stateMachine, PlayerMovement movementControl)
        : base("wallrun", stateMachine, movementControl) { }

    private void ChangeVelocity()
    {
        Vector3 normal = data.WallNormal.right == Vector3.zero ? data.WallNormal.left : -data.WallNormal.right;
        Vector3 moveDirectionVector3 = Vector3.Cross(normal, Vector3.up);
        Vector2 moveDirectionVector2 = new(moveDirectionVector3.x, moveDirectionVector3.z);

        Debug.Log($"{moveDirectionVector2} {moveDirectionVector3}");

        Vector2 desiredSpeed = stateMachine.fields.WallrunSpeed * moveDirectionVector2 * stateMachine.fields.SpeedMultiplier;

        data.horizontalMove =
            Vector2.Lerp(data.horizontalMove, desiredSpeed, stateMachine.fields.InterpolationRate * Time.deltaTime);
    }

    #region State logic

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        data.GetWalls(stateMachine);

        ChangeVelocity();

        CheckForChangeState();
    }

    protected override void CheckForChangeState()
    {
        if(data.gotJumpInput)
            stateMachine.ChangeState(stateMachine.midAir);
        else if (!gotWall)
        {
            if(!isGrounded)
                stateMachine.ChangeState(stateMachine.midAir);
            stateMachine.ChangeState(stateMachine.walk);
        }
    }

    #endregion

}