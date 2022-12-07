using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

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

            controls = new PlayerControls();
            controls.Player.Enable();

            controls.Player.Jump.performed += AddJump;
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
        {
            Vector2 inputVector = controls.Player.Move.ReadValue<Vector2>();
            Vector3 orientatedInputVector = stateMachine.fields.orientation.forward * inputVector.y + stateMachine.fields.orientation.right * inputVector.x;
            input = new Vector2(orientatedInputVector.x, orientatedInputVector.z);
        }

        private void CheckIfGrounded()
            => isGrounded = Physics.CheckSphere(stateMachine.fields.groundCheck.position, stateMachine.fields.groundCheckRadius, stateMachine.fields.groundCheckLM, QueryTriggerInteraction.Ignore);

        protected bool CheckIfMoving()
        {
            if(input != Vector2.zero)
                return data.horizontalMagnitude > 0.15f;
            return data.horizontalMagnitude < math.EPSILON;
        }

        private void AddJump(InputAction.CallbackContext context)
            => data.gotJumpInput = true;
    }
}