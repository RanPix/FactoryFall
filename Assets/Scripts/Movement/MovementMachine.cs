using UnityEngine;

namespace FiniteMovementStateMachine
{
    [RequireComponent(typeof(CharacterController), typeof(PlayerDataFields))]
    public class MovementMachine : MonoBehaviour
    {

        public BaseMovementState currentState { get; protected set; }
        [HideInInspector] public Idle idle { get; private set; }
        [HideInInspector] public Walk walk { get; private set; }
        [HideInInspector] public MidAir midAir { get; private set; }
        [HideInInspector] public Run run { get; private set; }
        [HideInInspector] public Wallrun wallrun { get; private set; }

        protected void Start()
        {
            InitializeStates();

            currentState = GetInitialState();
            currentState?.Enter(new MovementDataIntersection());
        }

        private void Update()
        {
            Debug.Log($"Im in {currentState}",this);

            currentState?.UpdateLogic();
            currentState?.CheckForChangeState();
            currentState?.UpdatePhysics();
        }


        private void InitializeStates()
        {
            PlayerMovement movementControl = new(gameObject.GetComponent<CharacterController>());
            PlayerDataFields fields = GetComponent<PlayerDataFields>();

            idle = new(this, movementControl, fields);
            walk = new(this, movementControl, fields);
            midAir = new(this, movementControl, fields);
            run = new(this, movementControl, fields);
            wallrun = new(this, movementControl, fields);
        }

        private BaseMovementState GetInitialState()
            => idle;

        public void ChangeState(BaseMovementState newState)
        {
            var data = currentState.Exit();

            currentState = newState;
            currentState.Enter(data);
        }
    }
}