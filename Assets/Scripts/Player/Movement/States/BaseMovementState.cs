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
        private readonly PlayerMovement movementControl;

        protected bool isGrounded;

        public BaseMovementState(string name, MovementMachine stateMachine, PlayerMovement movementControl)
        {
            this.name = name;
            this.stateMachine = stateMachine;
            this.movementControl = movementControl;
        }

        #region State Logic

        public virtual void Enter(MovementDataIntersection inputData)
        {
            data = inputData;
        }

        public virtual void UpdateLogic()
        {
            GetInput();
            CheckIfGrounded();
        }

        public virtual void UpdatePhysics()
        {
            movementControl.Move(data.GetMoveVector3());
        }

        public virtual MovementDataIntersection Exit()
        {
            return data;
        }

        #endregion

        private void GetInput()
            => input = controls.Player.Move.ReadValue<Vector2>().normalized;

        private void CheckIfGrounded()
            => isGrounded = Physics.CheckSphere(DataFields.groundCheck.position, DataFields.groundCheckRadius, DataFields.groundCheckLM, QueryTriggerInteraction.Ignore);

    }
}