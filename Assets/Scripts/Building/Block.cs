using UnityEngine;

public class Block : MonoBehaviour, IDamagable
{
    [Header("Stats")]

    [SerializeField] private bool canBeDestroyed = true;
    [SerializeField] private bool support = false;

    [Space]
    [SerializeField] private float defence;
    [SerializeField] private float damageCut;
    [SerializeField] private float health;

    [Space]
    [SerializeField] private float timeToRemove;

    [Header("yes")]
    [SerializeField] private bool canRecieveEnergy;
    [SerializeField] protected float energy;

    public void Damage(float damage)
    {
        if (!canBeDestroyed)
            return;

        if (defence > damage)
            damage = Mathf.Clamp(damage, 1, damage - damageCut);

        health -= damage;

        if (health < 0)
            Destroy(gameObject);
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
