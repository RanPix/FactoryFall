using UnityEngine;

public class Ore : Block
{
    [Header("Ore")]

    [SerializeField] protected float dmgToDropItem;
    protected float dmgDone;

    [SerializeField] protected float item;

    public float GetDamageToDropItem() => dmgToDropItem;
}
