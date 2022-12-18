using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

namespace FiniteMovementStateMachine
{
    [CreateAssetMenu(fileName = "PlayerDataFields", menuName = "ScriptableObject/PlayerDataFields")]
    public class PlayerDataFields : ScriptableObject
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
        
        [Space] 

        public float MaxAirSpeed;
        public float AirSpeed;

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

    public struct MovementDataIntersection
    {
        public Vector2 horizontalMove;
        public float verticalMove;

        public (Vector3 right, Vector3 left) WallNormal;
        public Vector3 lastWallNormal;

        public bool gotJumpInput;

        public Vector3 moveVector3 
            => new(horizontalMove.x, verticalMove, horizontalMove.y);

        public float horizontalMagnitude { get; private set; }

        public void CalculateHorizontalMagnitude()
            => horizontalMagnitude = horizontalMove.magnitude;

        public bool IsMovingHorizontally()
          => horizontalMove != Vector2.zero;

        public bool GetWalls(MovementMachine stateMachine) // Im ready to die for this one TwT
        {
            Transform orientation = stateMachine.orientation;
            RaycastHit hitInfo;
            bool foundWall = false;

            WallNormal = (Vector3.zero, Vector3.zero);

            if (Physics.Raycast(orientation.position, orientation.right, out hitInfo,
            stateMachine.fields.WallrunRayCheckDistance, stateMachine.fields.WallCheckLm,
                    QueryTriggerInteraction.Ignore))
            {
                WallNormal.right = hitInfo.normal;
                foundWall = true;
            }

            if (Physics.Raycast(orientation.position, -orientation.right, out hitInfo,
            stateMachine.fields.WallrunRayCheckDistance, stateMachine.fields.WallCheckLm,
                    QueryTriggerInteraction.Ignore))
            {
                WallNormal.left = hitInfo.normal;
                foundWall = true;
            }

            return foundWall;
        }

        public bool CheckForWalls(MovementMachine stateMachine) // For this one even more
            => Physics.Raycast(stateMachine.orientation.position, stateMachine.orientation.right,
                   stateMachine.fields.WallrunRayCheckDistance, stateMachine.fields.WallCheckLm,
                   QueryTriggerInteraction.Ignore)
               ||
               Physics.Raycast(stateMachine.orientation.position, -stateMachine.orientation.right,
                   stateMachine.fields.WallrunRayCheckDistance, stateMachine.fields.WallCheckLm,
                   QueryTriggerInteraction.Ignore);


    }
}