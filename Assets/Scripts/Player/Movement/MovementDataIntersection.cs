using FiniteStateMachine;
using UnityEngine;

namespace FiniteStateMachine
{
    public interface IFSMData
    {

    }
}

namespace FiniteMovementStateMachine
{
    public struct MovementDataIntersection : IFSMData
    {
        public Vector2 horizontalMove;
        public float verticalMove;
        public float horizontalMagnitude { get; private set; }

        public void CalculateHorizontalMagnitude()
            => horizontalMagnitude = horizontalMove.magnitude;

        public Vector3 GetMoveVector3()
            => new(horizontalMove.x, verticalMove, horizontalMove.y);
    }
}