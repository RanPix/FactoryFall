using UnityEngine;

namespace FiniteMovementStateMachine
{
    [RequireComponent(typeof(CharacterController), typeof(PlayerDataFields))]
    public class MovementMachine : MonoBehaviour
    {

        [HideInInspector] public BaseMovementState currentState { get; protected set; }
        [HideInInspector] public Idle idle { get; private set; }
        [HideInInspector] public Walk walk { get; private set; }
        [HideInInspector] public MidAir midAir { get; private set; }
        [HideInInspector] public Run run { get; private set; }
        [HideInInspector] public Wallrun wallrun { get; private set; }

        protected void Start()
        {
            InitializeStates();

            currentState = GetInitialState();
            currentState?.Enter();
        }

        private void Update()
        {
            //Debug.Log($"Im in {currentState}",this);

            currentState?.UpdateLogic();
            currentState?.CheckForChangeState();

            currentState?.UpdatePhysics();
        }


        private void InitializeStates()
        {
            PlayerMovement movementControl = new(gameObject.GetComponent<CharacterController>());
            PlayerDataFields fields = GetComponent<PlayerDataFields>();
            MovementDataIntersection data = new();

            idle = new(this, movementControl, fields, data);
            walk = new(this, movementControl, fields, data);
            midAir = new(this, movementControl, fields, data);
            run = new(this, movementControl, fields, data);
            wallrun = new(this, movementControl, fields, data);
        }

        private BaseMovementState GetInitialState()
            => idle;

        public void ChangeState(BaseMovementState newState)
        {
            currentState.Exit();

            currentState = newState;
            currentState.Enter();
        }
    }
}