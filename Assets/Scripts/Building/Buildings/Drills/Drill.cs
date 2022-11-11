using UnityEngine;

public class Drill : Block
{
    [Header("Drill")]

    [SerializeField] protected LayerMask oreLM;

    [Space]

    [SerializeField] protected Transform diggingPos;
    [SerializeField] protected float diggingRadius;
    [SerializeField] protected float diggingTime;
    protected float digTimer;

    protected float damageDone;

    [Space]

    [SerializeField] protected float diggingStrength;

    [Space]

    [SerializeField] protected int inventory;
}
