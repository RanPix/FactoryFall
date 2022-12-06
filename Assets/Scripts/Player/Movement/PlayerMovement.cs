using UnityEngine;

namespace FiniteMovementStateMachine
{
    public class PlayerMovement
    {
        private readonly CharacterController controller;

        public PlayerMovement(CharacterController controller)
        {
            this.controller = controller;
        }

        public void Move(Vector3 direction)
            => controller.Move(direction);
    }
}