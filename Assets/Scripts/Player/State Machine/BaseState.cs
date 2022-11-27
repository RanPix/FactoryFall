namespace FiniteStateMachine
{
    public class BaseState
    {
        public string name { get; private set; }
        protected StateMachine stateMachine;

        public BaseState(string name, StateMachine stateMachine)
        {
            this.name = name;
            this.stateMachine = stateMachine;
        }

        public virtual void Enter() { }
        public virtual void UpdateLogic() { }
        public virtual void UpdatePhysics() { }
        public virtual void Exit() { }
    }
}