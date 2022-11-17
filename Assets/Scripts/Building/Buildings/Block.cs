using UnityEngine;
using Mirror;

[RequireComponent(typeof(PlacedObject))]
public class Block : NetworkBehaviour, IDamagable
{
    [SerializeField] protected string blockName;

    [Header("Stats")]

    [SerializeField] protected bool isDestroyable = true;

    [Space]
    [SerializeField] protected float defence;
    [SerializeField] protected float damageCut;
    [SyncVar][SerializeField] private float health;

    [Space]
    [SerializeField] private float timeToRemove;

    [Header("Energy")]
    [SerializeField] private bool canRecieveEnergy;
    [SyncVar][SerializeField] protected float energy;

    public virtual bool Damage(float damage)
    {
        if (!isDestroyable)
            return false;

        if (defence > damage)
            damage = Mathf.Clamp(damage, 1, damage - damageCut);

        health -= damage;

        if (health < 0)
        {
            Destroy(gameObject);
            return true;
        }

        return false;
    }

    public void AddEnergy(float amount)
    {
        if (!canRecieveEnergy)
            return;

        energy += amount;
    }

    public void RemoveBlock()
    {
        Destroy(gameObject);
    }

    public float GetRemoveTime() => timeToRemove;
}
