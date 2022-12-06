using UnityEngine;

namespace FiniteMovementStateMachine
{
    [System.Serializable]
    public static class DataFields
    {
        [Header("MoveSpeeds")]
        public static readonly float speedMultiplier;
        public static readonly float interpolationRate;
        [Space]
        public static readonly float walkSpeed;
        [Header("Checks")]
        public static readonly Transform groundCheck;
        public static readonly float groundCheckRadius;
        public static readonly LayerMask groundCheckLM;
        [Space]
        public static readonly Transform ceilingCheck;
        public static readonly float ceilingCheckRadius;
        public static readonly LayerMask ceilingCheckLM;
    }

    public struct MovementDataIntersection
    {
        public Vector2 horizontalMove;
        public float verticalMove;

        public bool gotJumpInput;


        public float horizontalMagnitude { get; private set; }

        public void CalculateHorizontalMagnitude()
            => horizontalMagnitude = horizontalMove.magnitude;

        public Vector3 GetMoveVector3()
            => new(horizontalMove.x, verticalMove, horizontalMove.y);
    }
}