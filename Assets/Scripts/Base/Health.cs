using Mirror;
using UnityEngine;
using System;

namespace GameBase
{
    public class Health : NetworkBehaviour
    {
        [field: SerializeField] public float maxHealth { get; private set; }
        [field: SyncVar] public float currentHealth { get; private set; }

        public Action onDeath;

        private void Start()
            => currentHealth = maxHealth;
        public void Damage(Damage damage)
        {
            currentHealth -= damage.damage;

                print("nuuuuuuuuuuuuuuuuuuuuuuuuuul");
            if (currentHealth < 1)
            {
                print("death");
                onDeath?.Invoke();
            }
        }

        public void Regen(float amount)
        {
            currentHealth = Mathf.Clamp(currentHealth + amount, 1, maxHealth);
        }
    }
}
