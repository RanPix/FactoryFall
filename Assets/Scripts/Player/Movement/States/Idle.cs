using FiniteMovementStateMachine;

public class Idle : BaseMovementState
{
    public Idle(MovementMachine stateMachine) : base("Idle", stateMachine) { }

    public override void Enter()
    {
        
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        
    }

    public override void UpdatePhysics()
    {

    }

    public override void Exit()
    {

    }
}
