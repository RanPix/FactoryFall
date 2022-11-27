using Mirror;

namespace FiniteMovementStateMachine
{
    public class MovementMachine : NetworkBehaviour
    {
        private bool notLocalPlayer;

        protected BaseMovementState currentState;
        private Idle idle;
        private Walk walk;
        

        private void Awake()
        {
            notLocalPlayer = !isLocalPlayer;

            idle = new Idle(this);
            walk = new Walk(this);
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