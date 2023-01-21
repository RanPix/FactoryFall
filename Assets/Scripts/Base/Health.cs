using Mirror;
using UnityEngine;
using System;

namespace GameBase
{
    public class Health : NetworkBehaviour
    {
        [field: SerializeField] public float maxHealth { get; private set; }

        [field: SyncVar] public float currentHealth { get; private set; }

        //[field: SyncVar] public float gotDamage { get; set; }
        public Action OnHealthChanged;
        public Action<string> onDeath;

        private void Start()
        {
            currentHealth = maxHealth;
        }

       //[TargetRpc]
        public void Damage(string playerID, int damage)
        {
            currentHealth -= damage;

            OnHealthChanged?.Invoke();
            //print($"damage: {damage}");

            CheckHealth(playerID);
        }

        public void CheckHealth(string playerID)
        {
            print($"health: {currentHealth}");

            if (currentHealth < 1)
            {
                print("death");
                onDeath?.Invoke(playerID);

            }
        }

        public void Regen(float amount)
        {
            currentHealth = Mathf.Clamp(currentHealth + amount, 1, maxHealth);
        }

        public void SetHealth(float health)
        {
            currentHealth = health;
        }
    }
}
