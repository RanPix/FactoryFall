using Mirror;

namespace FiniteStateMachine
{
    public class StateMachine : NetworkBehaviour
    {
        private BaseState currentState;


        protected void Start()
        {
            currentState = GetInitialState();
            currentState?.Enter();
        }

        protected void Update()
        {
            currentState?.UpdateLogic();
        }

        protected void LateUpdate()
        {
            currentState?.UpdatePhysics();
        }

        protected virtual BaseState GetInitialState()
        {
            return null;
        }

        public void ChangeState(BaseState newState)
        {
            currentState.Exit();

            currentState = newState;
            currentState.Enter();
        }
    }
}

