using UnityEngine;
using FiniteStateMachine;
using Unity.Mathematics;

namespace FiniteMovementStateMachine
{
    public class BaseMovementState : BaseState
    {
        protected new MovementMachine stateMachine;
        protected PlayerControls controls;
        protected Vector3 input;

        public BaseMovementState(string name, MovementMachine stateMachine) : base(name, stateMachine)
        {
            
        }

        public override void UpdateLogic()
        {
            GetInput();
        }

        protected void GetInput()
        {
            Vector2 controlInput = controls.Player.Move.ReadValue<Vector2>();
            input = new Vector3(controlInput.x, 0, controlInput.y).normalized;
        }
    }
}