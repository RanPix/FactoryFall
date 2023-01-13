using UnityEngine;

namespace FiniteMovementStateMachine
{
    [CreateAssetMenu(fileName = "PlayerScriptableDataFields", menuName = "ScriptableObject/PlayerScriptableDataFields")]
    public class PlayerScriptableDataFields : ScriptableObject
    {
        [Header("Move Speeds")] 

        public float SpeedMultiplier;
        public float InterpolationRate;

        [Space]

        public float WalkSpeed;
        public float RunSpeed;
        public float WallrunSpeed;

        [Header("Vertical movement")] 

        public float JumpHeight;

        public bool JumpOverlap;
        public int DoubleJumps;
        public float Gravity;
        public float MaxFallSpeed;
        
        [Space] 

        public float MaxAirSpeed;
        public float AirSpeed;

        [Header("Wallrun")]

        public float MaxWallrunDistance;
        public float WallrunFallOffSpeedMultiplier;
        public float WallrunFallOffDirctionMultiplier; // Bigger number == sharper angle
        public float MaxWallrunBoostSpeed;

        [Header("Redirect")] 
        
        public int Redirects;

        [Header("Checks")] 
        
        public float GroundCheckRadius;
        public LayerMask GroundCheckLm;

        [Space]
        
        public float CeilingCheckRadius;
        public LayerMask CeilingCheckLm;

        [Space]

        public float WallrunRayCheckDistance;
        public LayerMask WallCheckLm;
    }

    public class MovementDataIntersection
    {
        public Vector2 horizontalMove;
        public float verticalMove;

        public (Vector3 right, Vector3 left) WallNormals;
        public Vector3 lastWallNormal;

        public Vector3 deltaMove;

        public bool gotJumpInput;

        public Vector3 moveVector3 
            => new(horizontalMove.x, verticalMove, horizontalMove.y);

        public float horizontalMagnitude { get; private set; }

        public void CalculateHorizontalMagnitude()
            => horizontalMagnitude = horizontalMove.magnitude;

        public bool IsMovingHorizontally()
          => horizontalMove != Vector2.zero;

        public bool GetWalls(PlayerDataFields fields)
        {
            RaycastHit hitInfo;
            bool foundWall = false;

            WallNormals = (Vector3.zero, Vector3.zero);

            if (Physics.Raycast(fields.wallCheck.position, fields.orientation.right, out hitInfo,
                    fields.ScriptableFields.WallrunRayCheckDistance, fields.ScriptableFields.WallCheckLm,
                    QueryTriggerInteraction.Ignore))
            {
                WallNormals.right = hitInfo.normal;
                foundWall = true;
            }

            if (Physics.Raycast(fields.wallCheck.position, -fields.orientation.right, out hitInfo,
                    fields.ScriptableFields.WallrunRayCheckDistance, fields.ScriptableFields.WallCheckLm,
                    QueryTriggerInteraction.Ignore))
            {
                WallNormals.left = hitInfo.normal;
                foundWall = true;
            }

            return foundWall;
        }
    }
}