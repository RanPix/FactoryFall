using FiniteMovementStateMachine;
using UnityEngine;

public class Wallrun : BaseMovementState
{
    private float wallrunDistance;
    private Vector3 currentWallNormal;
    private Vector2 moveDirectionVector2;

    public Wallrun(MovementMachine stateMachine, PlayerMovement movementControl, PlayerDataFields fields)
        : base("wallrun", stateMachine, movementControl, fields) { }

    #region State logic

    public override void Enter(MovementDataIntersection inputData)
    {
        base.Enter(inputData);

        data.GetWalls(fields);

        CalculateMoveDirection();

        wallrunDistance = fields.ScriptableFields.MaxWallrunDistance;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        data.GetWalls(fields);

        ChangeVelocity();
    }

    public override void CheckForChangeState()
    {

        if (data.gotJumpInput || CheckIfMovingOfWall())
            stateMachine.ChangeState(stateMachine.midAir);

        else if (!GetGotWall())
        {
            if(!GetIsGrounded())
                stateMachine.ChangeState(stateMachine.midAir);
            stateMachine.ChangeState(stateMachine.walk);
        }
    }

    public override MovementDataIntersection Exit()
    {
        data.lastWallNormal = currentWallNormal;

        return base.Exit();
    }

    #endregion

    private void ChangeVelocity()
    {
        Vector2 desiredSpeed = fields.ScriptableFields.WallrunSpeed * moveDirectionVector2 * fields.ScriptableFields.SpeedMultiplier;

        data.horizontalMove =
            Vector2.Lerp(data.horizontalMove, desiredSpeed, fields.ScriptableFields.InterpolationRate * Time.deltaTime);
    }

    private void CalculateMoveDirection()
    {
        Vector3 moveDirectionVector3;

        if (data.WallNormals.left != Vector3.zero && data.WallNormals.left != data.lastWallNormal)
        {
            currentWallNormal = data.WallNormals.left;
            moveDirectionVector3 = Vector3.Cross(currentWallNormal, Vector3.up);
        }
        else
        {
            currentWallNormal = data.WallNormals.right;
            moveDirectionVector3 = Vector3.Cross(-currentWallNormal, Vector3.up);
        } 

        moveDirectionVector2 = new(moveDirectionVector3.x, moveDirectionVector3.z);
    }

    private bool CheckIfMovingOfWall()
    {
        float angle = Quaternion.LookRotation(input).eulerAngles.z - Quaternion.LookRotation(currentWallNormal).eulerAngles.y;
        angle = angle < 0 ? -angle : angle; // Handmade Abs)

        return angle is < 50 or > 310;
    }

    private void DeleteOldWallNormal()
        => data.lastWallNormal = Vector3.zero;
}