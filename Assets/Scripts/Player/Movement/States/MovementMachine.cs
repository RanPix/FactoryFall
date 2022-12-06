using Mirror;
using UnityEngine;

namespace FiniteMovementStateMachine
{
    [RequireComponent(typeof(CharacterController))]
    public class MovementMachine : NetworkBehaviour
    {
        private bool notLocalPlayer;

        protected BaseMovementState currentState;
        [HideInInspector] public Idle idle { get; private set; }
        [HideInInspector] public Walk walk { get; private set; }
        [HideInInspector] public MidAir midAir { get; private set; }

        private void Awake()
        {
            notLocalPlayer = !isLocalPlayer;

            PlayerMovement movementControl = new(gameObject.GetComponent<CharacterController>());

            idle = new(this, movementControl);
            walk = new(this, movementControl);
            midAir = new(this, movementControl);
        }


        protected void Start()
        {
            if (notLocalPlayer)
                return;

            currentState = GetInitialState();
            currentState?.Enter(new MovementDataIntersection());
        }

        protected void Update()
        {
            if (notLocalPlayer)
                return;

            currentState?.UpdateLogic();
        }

        protected void LateUpdate()
        {
            if (notLocalPlayer)
                return;

            currentState?.UpdatePhysics();
        }

        protected BaseMovementState GetInitialState()
            => idle;

        public void ChangeState(BaseMovementState newState)
        {
            var data = currentState.Exit();

            currentState = newState;
            currentState.Enter(data);
        }
    }
}