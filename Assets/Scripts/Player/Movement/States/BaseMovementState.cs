using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FiniteMovementStateMachine
{
    public class BaseMovementState
    {
        public readonly string name;

        protected readonly PlayerDataFields fields;
        protected readonly MovementMachine stateMachine; // State machine state is attached to
        protected readonly PlayerControls controls; // Input system
        private readonly PlayerMovement movementControl; // Character controller container

        protected MovementDataIntersection data;
        protected Vector2 input { get; private set; }

        protected bool gotWall { get; private set; }
        protected bool isGrounded { get; private set; }
        protected bool isMovingForward { get; private set; }

        public BaseMovementState(string name, MovementMachine stateMachine, PlayerMovement movementControl, PlayerDataFields fields)
        {
            this.name = name;
            this.stateMachine = stateMachine;
            this.movementControl = movementControl;
            this.fields = fields;

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
            CheckForWalls();
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

        protected void GetInput()
        {
            Vector2 inputVector = controls.Player.Move.ReadValue<Vector2>();
            Vector3 orientatedInputVector = fields.orientation.forward * inputVector.y + fields.orientation.right * inputVector.x;
            input = new Vector2(orientatedInputVector.x, orientatedInputVector.z);
        }

        protected void CheckIfGrounded()
            => isGrounded = Physics.CheckSphere(fields.groundCheck.position, fields.ScriptableFields.GroundCheckRadius, fields.ScriptableFields.GroundCheckLm, QueryTriggerInteraction.Ignore);

        protected bool CheckIfHaveInput()
            => input != Vector2.zero;

        private void AddJump(InputAction.CallbackContext context) // Adds jump to queue while changing states
            => data.gotJumpInput = true;

        protected void CheckForWalls()
            => gotWall = Physics.Raycast(fields.wallCheck.position, fields.orientation.right,
                             fields.ScriptableFields.WallrunRayCheckDistance, fields.ScriptableFields.WallCheckLm,
                   QueryTriggerInteraction.Ignore)
               ||
               Physics.Raycast(fields.wallCheck.position, -fields.orientation.right,
                   fields.ScriptableFields.WallrunRayCheckDistance, fields.ScriptableFields.WallCheckLm,
                   QueryTriggerInteraction.Ignore);

        protected void CheckIfMovingForward()
        {
            float angle = Quaternion.LookRotation(data.moveVector3).eulerAngles.y - fields.orientation.eulerAngles.y;
            angle = angle < 0 ? -angle : angle; // Handmade Abs)

            isMovingForward = angle is < 45.1f or > 314.9f;
        }
    }
}