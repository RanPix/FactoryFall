using FiniteMovementStateMachine;

public class Walk : BaseMovementState
{
    public Walk(MovementMachine stateMachine) : base("Walk", stateMachine) { }

    public override void Enter(MovementDataIntersection data)
    {
        base.Enter(data);


    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }
}
