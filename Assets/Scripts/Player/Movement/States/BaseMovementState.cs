using UnityEngine;

namespace FiniteMovementStateMachine
{
    public class BaseMovementState
    {
        public string name { get; private set; }

        protected MovementDataIntersection data;

        protected MovementMachine stateMachine;
        protected PlayerControls controls;
        protected Vector2 input;
        protected PlayerMovement movement;

        public BaseMovementState(string name, MovementMachine stateMachine)
        {
            this.name = name;
            this.stateMachine = stateMachine;

            movement = stateMachine.gameObject.GetComponent<PlayerMovement>();
        }

        #region State Logic

        public virtual void Enter(MovementDataIntersection inputData)
        {
            data = inputData;
        }

        public virtual void UpdateLogic()
        {
            GetInput();
        }

        public virtual void UpdatePhysics()
        {
            movement.Move(data.GetMoveVector3());
        }

        public virtual MovementDataIntersection Exit()
        {
            return data;
        }

        #endregion

        private void GetInput()
            => input = controls.Player.Move.ReadValue<Vector2>().normalized;
    }
}