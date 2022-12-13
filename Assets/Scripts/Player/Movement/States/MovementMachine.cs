using Mirror;
using UnityEngine;

namespace FiniteMovementStateMachine
{
    [RequireComponent(typeof(CharacterController))]
    public class MovementMachine : NetworkBehaviour
    {
        [field: SerializeField] public PlayerDataFields fields { get; private set; }
        [field: SerializeField] public Transform  orientation { get; private set; }
        [field: SerializeField] public Transform groundCheck { get; private set; }
        [field: SerializeField] public Transform ceilingCheck { get; private set; }

        private bool notLocalPlayer;

        public BaseMovementState currentState { get; protected set; }
        [HideInInspector] public Idle idle { get; private set; }
        [HideInInspector] public Walk walk { get; private set; }
        [HideInInspector] public MidAir midAir { get; private set; }
        [HideInInspector] public Run run { get; private set; }

        protected void Start()
        {
            notLocalPlayer = !isLocalPlayer;

            if (notLocalPlayer)
                return;

            PlayerMovement movementControl = new(gameObject.GetComponent<CharacterController>());

            idle = new(this, movementControl);
            walk = new(this, movementControl);
            midAir = new(this, movementControl);
            run = new(this, movementControl);

            currentState = GetInitialState();
            currentState?.Enter(new MovementDataIntersection());
        }

        protected void Update()
        {
            if (notLocalPlayer)
                return;
            //Debug.Log($"Im in {currentState}",this);

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