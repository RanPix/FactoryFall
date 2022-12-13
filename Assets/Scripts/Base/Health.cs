using Mirror;
using UnityEngine;
using System;

namespace GameBase
{
    public class Health : NetworkBehaviour
    {
        [field: SerializeField] public float maxHealth { get; private set; }
        [field: SyncVar] public float currentHealth { get; private set; }

        public Action onDeath { get; private set; }

        public void Damage(Damage damage)
        {
            currentHealth -= damage.damage;   

            if (currentHealth < 1)
                onDeath?.Invoke();
        }

        public void Regen(float amount)
        {
            currentHealth = Mathf.Clamp(currentHealth + amount, 1, maxHealth);
        }
    }
}
