using Mirror;
using UnityEngine;

namespace FiniteMovementStateMachine
{
    [RequireComponent(typeof(CharacterController))]
    public class MovementMachine : NetworkBehaviour
    {
        [SerializeField] public DataFields fields;
        
        private bool notLocalPlayer;

        protected BaseMovementState currentState;
        [HideInInspector] public Idle idle { get; private set; }
        [HideInInspector] public Walk walk { get; private set; }
        [HideInInspector] public MidAir midAir { get; private set; }

        protected void Start()
        {
            notLocalPlayer = !isLocalPlayer;

            PlayerMovement movementControl = new(gameObject.GetComponent<CharacterController>());

            idle = new(this, movementControl);
            walk = new(this, movementControl);
            midAir = new(this, movementControl);

            if (notLocalPlayer)
                return;

            currentState = GetInitialState();
            currentState?.Enter(new MovementDataIntersection());
        }

        protected void Update()
        {
            if (notLocalPlayer)
                return;
            Debug.Log($"Im in {currentState}",this);

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