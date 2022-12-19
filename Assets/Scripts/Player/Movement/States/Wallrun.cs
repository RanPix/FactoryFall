using FiniteMovementStateMachine;
using UnityEngine;

public class Wallrun : BaseMovementState
{
    public Wallrun(MovementMachine stateMachine, PlayerMovement movementControl, PlayerDataFields fields)
        : base("wallrun", stateMachine, movementControl, fields) { }

    #region State logic

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        data.GetWalls(fields);

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

    private void ChangeVelocity()
    {
        Vector3 normal = data.WallNormal.right == Vector3.zero ? data.WallNormal.left : -data.WallNormal.right;
        Vector3 moveDirectionVector3 = Vector3.Cross(normal, Vector3.up);
        Vector2 moveDirectionVector2 = new(moveDirectionVector3.x, moveDirectionVector3.z);

        Vector2 desiredSpeed = fields.ScriptableFields.WallrunSpeed * moveDirectionVector2 * fields.ScriptableFields.SpeedMultiplier;

        data.horizontalMove =
            Vector2.Lerp(data.horizontalMove, desiredSpeed, fields.ScriptableFields.InterpolationRate * Time.deltaTime);
    }
}