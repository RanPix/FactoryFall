using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metalon : Mob
{
    [SerializeField] private List<string> animatorStates = new List<string>();
    protected override float nextFire { get; }
    protected override GameObject targetGObj { get; }
    public override void ChooseTarget()
    {
    }
    public override void Shoot()
    {
        
    }

    [SerializeField] private List<string> attackAnimationsNames = new List<string>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Movement()
    {
        switch (state)
        {
            case MobStates.Aggressive:
                break;
            case MobStates.Calm:
                break;
            case MobStates.Atack:
                break;
            case MobStates.killed:
                break;
        }
    }

    public override void Animations()
    {

        switch (state)
        {
            case MobStates.Aggressive:
                mobAnimator.Play("Run Forward");
                break;
            case MobStates.Calm:
                mobAnimator.Play("Idle");
                break;
            case MobStates.Atack:

                mobAnimator.Play(attackAnimationsNames[Random.Range(0, attackAnimationsNames.Count)]);
                break;
            case MobStates.killed:
                break;
        }
    }
}
