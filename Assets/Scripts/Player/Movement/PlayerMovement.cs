using UnityEngine;

namespace FiniteMovementStateMachine
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement
    {
        private CharacterController controller;

        public PlayerMovement(CharacterController controller)
        {
            this.controller = controller;
        }

        public void Move(Vector3 direction)
            => controller.Move(direction);
    }
}