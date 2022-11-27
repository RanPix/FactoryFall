using UnityEngine;
using FiniteStateMachine;

[RequireComponent(typeof(CharacterController), typeof(AudioSource), typeof(Animator))]
public class MovementMachine : StateMachine
{
    private Idle idle;

    private void Awake()
    {
        idle = new Idle(this);
    }

    protected override BaseState GetInitialState()
    {
        return idle;
    }
}