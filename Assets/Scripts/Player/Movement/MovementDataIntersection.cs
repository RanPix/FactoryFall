using UnityEngine;

namespace FiniteMovementStateMachine
{
    [System.Serializable]
    public class DataFields
    {
        [field: Header("Move Speeds")]
        [field: SerializeField] public float speedMultiplier { get; private set; }
        [field: SerializeField] public float interpolationRate { get; private set; }


        [field: Space]
        [field: SerializeField] public float walkSpeed { get; private set; }
        [field: SerializeField] public float runSpeed { get; private set; }

        [field: Header("Vertical movement")]

        [field: SerializeField] public float jumpHeight { get; private set; }
        [field: SerializeField] public bool jumpOverlap { get; private set; }
        [field: SerializeField] public int doubleJumps { get; private set; }
        [field: SerializeField] public float gravity { get; private set; }


        [field: Space]
        [field: SerializeField] public float maxAirSpeed { get; private set; }
        [field: SerializeField, Range(0, 1)] public float airMultiplier { get; private set; }



        [field: Header("Checks")]
        [field: SerializeField] public Transform groundCheck { get; private set; }
        [field: SerializeField] public float groundCheckRadius { get; private set; }
        [field: SerializeField] public LayerMask groundCheckLM { get; private set; }


        [field: Space]
        [field: SerializeField] public Transform ceilingCheck { get; private set; }
        [field: SerializeField] public float ceilingCheckRadius { get; private set; }
        [field: SerializeField] public LayerMask ceilingCheckLM { get; private set; }


        [field: Header("Required")]
        [field: SerializeField] public Transform orientation { get; private set; }

    }

    public struct MovementDataIntersection
    {
        public Vector2 horizontalMove;
        public float verticalMove;

        public bool gotJumpInput;

        public Vector3 moveVector3 => new(horizontalMove.x, verticalMove, horizontalMove.y);

        public float horizontalMagnitude { get; private set; }

        public void CalculateHorizontalMagnitude()
            => horizontalMagnitude = horizontalMove.magnitude;
    }
}