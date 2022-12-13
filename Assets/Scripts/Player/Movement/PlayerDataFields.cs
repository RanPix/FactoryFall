using UnityEngine;

namespace FiniteMovementStateMachine
{
    [CreateAssetMenu(fileName = "PlayerDataFields", menuName = "ScriptableObject/PlayerDataFields")]
    public class PlayerDataFields : ScriptableObject
    {
        [Header("Move Speeds")] 

        public float speedMultiplier;
        public float interpolationRate;

        [Space]

        public float walkSpeed;
        public float runSpeed;

        [Header("Vertical movement")] 

        public float jumpHeight;

        public bool jumpOverlap;
        public int doubleJumps;
        public float gravity;
        
        [Space] 

        public float maxAirSpeed;
        public float airSpeed;

        [Header("Redirect")] 
        
        public int redirects;

        [Header("Checks")] 
        
        public float groundCheckRadius;
        public LayerMask groundCheckLM;

        [Space]
        
        public float ceilingCheckRadius;
        public LayerMask ceilingCheckLM;
    }

    public struct MovementDataIntersection
    {
        public Vector2 horizontalMove;
        public float verticalMove;

        public bool gotJumpInput;

        public Vector3 moveVector3 
            => new(horizontalMove.x, verticalMove, horizontalMove.y);

        public float horizontalMagnitude { get; private set; }

        public void CalculateHorizontalMagnitude()
            => horizontalMagnitude = horizontalMove.magnitude;

        public bool IsMovingHorizontally()
            => horizontalMove.x > 0 || horizontalMove.y > 0;
    }
}