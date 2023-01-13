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
        if (data.gotJumpInput || CheckIfMovingOfWall() || !GetGotWall() || wallrunDistance < 0)
            stateMachine.ChangeState(stateMachine.midAir);
    }

    public override MovementDataIntersection Exit()
    {
        ChangeVelocityToMoveOfWall();
        data.lastWallNormal = currentWallNormal;

        return base.Exit();
    }

    #endregion

    private void ChangeVelocity()
    {
        Vector2 desiredSpeed = fields.ScriptableFields.WallrunSpeed * moveDirectionVector2 * fields.ScriptableFields.SpeedMultiplier;

        data.horizontalMove =
            Vector2.Lerp(data.horizontalMove, desiredSpeed, fields.ScriptableFields.InterpolationRate * Time.deltaTime);

        data.CalculateHorizontalMagnitude();

        wallrunDistance -= data.horizontalMagnitude * Time.deltaTime;
    }

    private void ChangeVelocityToMoveOfWall()
    {
        data.CalculateHorizontalMagnitude();

        data.horizontalMove = (moveDirectionVector2 * fields.ScriptableFields.WallrunFallOffDirctionMultiplier + input)
                              .normalized *
                              data.horizontalMagnitude;

        if (data.horizontalMagnitude < fields.ScriptableFields.MaxWallrunBoostSpeed) // Check for speed limit
            data.horizontalMove *= fields.ScriptableFields.WallrunFallOffSpeedMultiplier;
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

        moveDirectionVector2 = new(moveDirectionVector3.x, moveDirectionVector3.z); // Is normalized
    }

    private bool CheckIfMovingOfWall()
    {
        float angle = Quaternion.LookRotation(currentWallNormal).eulerAngles.y - Quaternion.LookRotation(new(input.x, 0f, input.y)).eulerAngles.y;
        angle = angle < 0 ? -angle : angle; // Handmade Abs)

        return angle is < 50 or > 310;
    }
}