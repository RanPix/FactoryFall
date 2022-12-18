using System;
using System.Reflection;
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

        protected bool gotWall;

        protected bool isGrounded;
        protected bool isMovingForward;

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

        /// <summary>
        ///     Called once on start of state <br/>
        ///     In case of override base should be put at the start of method
        /// </summary>
        public virtual void Enter(MovementDataIntersection inputData)
        {
            data = inputData;
        }

        /// <summary>
        ///     Called every frame before Update Physics. Should be used only for calculation and changes between states <br/>
        ///     In case of override base should be put at the start of method
        /// </summary>
        public virtual void UpdateLogic()
        {
            GetInput();
            CheckIfGrounded();
            CheckIfMovingForward();

            gotWall = data.CheckForWalls(stateMachine);
        }

        /// <summary>
        ///     Called every frame after Update Logic. Should be only overrided in case of special movement situations
        /// </summary>
        public virtual void UpdatePhysics()
        {
            movementControl.Move(data.moveVector3);
        }

        /// <summary>
        ///     Called on exit of state <br/>
        ///     In case of override base should be put in the end of method
        /// </summary>
        public virtual MovementDataIntersection Exit()
        {
            return data;
        }

        /// <summary> Don't override with base </summary>
        protected virtual void CheckForChangeState()
        {
            Debug.LogWarning($"I was in {name} for a brief moment.\n  Override {MethodBase.GetCurrentMethod()?.Name} method");
            throw new NotImplementedException();
        }

        #endregion

        private void GetInput()
        {
            Vector2 inputVector = controls.Player.Move.ReadValue<Vector2>();
            Vector3 orientatedInputVector = stateMachine.orientation.forward * inputVector.y + stateMachine.orientation.right * inputVector.x;
            input = new Vector2(orientatedInputVector.x, orientatedInputVector.z);
        }

        private void CheckIfGrounded()
            => isGrounded = Physics.CheckSphere(stateMachine.groundCheck.position, stateMachine.fields.GroundCheckRadius, stateMachine.fields.GroundCheckLm, QueryTriggerInteraction.Ignore);

        protected bool CheckIfHaveInput()
            => input != Vector2.zero;

        private void AddJump(InputAction.CallbackContext context)
            => data.gotJumpInput = true;

        private void CheckIfMovingForward()
        {
            float angle = Quaternion.LookRotation(data.moveVector3).eulerAngles.y - stateMachine.orientation.eulerAngles.y;
            angle = angle < 0 ? -angle : angle; // Handmade Abs)

            isMovingForward = angle is < 45.1f or > 314.9f;
        }
    }
}