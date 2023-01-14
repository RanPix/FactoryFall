using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FiniteMovementStateMachine
{
    public abstract class BaseMovementState
    {
        public readonly string name;

        protected readonly PlayerDataFields fields;
        protected readonly MovementMachine stateMachine; // State machine state is attached to
        protected readonly PlayerControls controls; // Input system
        private readonly PlayerMovement movementControl; // Character controller container

        protected MovementDataIntersection data;
        protected Vector2 input { get; private set; }

        private bool gotWall;
        private bool gotWallIsUpdatedThisFrame;
        
        private bool isGrounded;
        private bool isGroundedIsUpdatedThisFrame;

        private bool isMovingForward;
        private bool isMovingForwardIsUpdatedThisFrame;

        #region Getters

        protected bool GetGotWall()
        {
            if (gotWallIsUpdatedThisFrame)
                return gotWall;

            gotWall = CheckForWalls();
            gotWallIsUpdatedThisFrame = true;

            return gotWall;
        }

        protected bool GetIsGrounded()
        {
            if (isGroundedIsUpdatedThisFrame)
                return isGrounded;

            isGrounded = CheckIfGrounded();
            isGroundedIsUpdatedThisFrame = true;

            return isGrounded;
        }

        protected bool GetIsMovingForward()
        {
            if (isMovingForwardIsUpdatedThisFrame)
                return isMovingForward;

            isMovingForward = CheckIfMovingForward();
            isMovingForwardIsUpdatedThisFrame = true;

            return isMovingForward;
        }

        #endregion

        internal BaseMovementState(string name, MovementMachine stateMachine, PlayerMovement movementControl, PlayerDataFields fields, MovementDataIntersection data)
        {
            this.name = name;
            this.stateMachine = stateMachine;
            this.movementControl = movementControl;
            this.fields = fields;
            this.data = data;

            controls = new PlayerControls();
            controls.Player.Enable();

            controls.Player.Jump.performed += AddJump;
        }

        #region State Logic

        /// <summary>
        ///     Called once on start of state
        /// </summary>
        public virtual void Enter() { }

        /// <summary>
        ///     Called every frame before Update Physics. Should be used only for calculation and changes between states <br/>
        ///     In case of override base should be put at the start of method
        /// </summary>
        public virtual void UpdateLogic()
        {
            GetInput();

            gotWallIsUpdatedThisFrame = false;
            isGroundedIsUpdatedThisFrame = false;
            isMovingForwardIsUpdatedThisFrame = false;
        }

        /// <summary>
        ///     Called every frame after Update Logic. Should be only overrided in case of special movement situations
        /// </summary>
        public virtual void UpdatePhysics()
        {
            movementControl.Move(data.moveVector3);
        }

        /// <summary>
        ///     Called on exit of state
        /// </summary>
        public virtual void Exit() { }

        /// <summary> Don't override with base </summary>
        public virtual void CheckForChangeState()
        {
            Debug.LogWarning($"I was in {name} for a brief moment.\n\t   Override {MethodBase.GetCurrentMethod()?.Name} method");
            throw new NotImplementedException();
        }

        #endregion

        #region Checks

        private void GetInput()
        {
            Vector2 inputVector = controls.Player.Move.ReadValue<Vector2>();
            Vector3 orientatedInputVector = fields.orientation.forward * inputVector.y + fields.orientation.right * inputVector.x;
            input = new Vector2(orientatedInputVector.x, orientatedInputVector.z);
        }

        private bool CheckIfGrounded()
            => isGrounded = Physics.CheckSphere(fields.groundCheck.position, fields.ScriptableFields.GroundCheckRadius,
                fields.ScriptableFields.GroundCheckLm, QueryTriggerInteraction.Ignore);

        private void AddJump(InputAction.CallbackContext context) // Adds jump to queue while changing states
            => data.gotJumpInput = true;

        private bool CheckForWalls()
        {
            RaycastHit hit;
            bool foundWall;

            foundWall = Physics.Raycast(fields.wallCheck.position, fields.orientation.right, out hit, // Right
                fields.ScriptableFields.WallrunRayCheckDistance, fields.ScriptableFields.WallCheckLm,
                QueryTriggerInteraction.Ignore);
            
            if(foundWall)
                foundWall = hit.normal != data.lastWallNormal;

            if (foundWall)
                return true;

            foundWall = Physics.Raycast(fields.wallCheck.position, -fields.orientation.right, out hit, // Left
                           fields.ScriptableFields.WallrunRayCheckDistance, fields.ScriptableFields.WallCheckLm,
                           QueryTriggerInteraction.Ignore);
            if (foundWall)
                foundWall = hit.normal != data.lastWallNormal;

            return foundWall;
        }

        private bool CheckIfMovingForward()
        {
            if(data.moveVector3 == Vector3.zero)
                return false;

            float angle = Quaternion.LookRotation(data.moveVector3).eulerAngles.y - fields.orientation.eulerAngles.y;
            angle = angle < 0 ? -angle : angle; // Handmade Abs)

            return angle is < 50 or > 310;
        }

        #endregion
    }
}