using Mirror;
using UnityEngine;
using System;

namespace GameBase
{
    public class Health : NetworkBehaviour
    {
        [SerializeField] private HealthPreset healthValues;

        [field: SyncVar] public float currentHealth { get; private set; }

        public Action OnHealthChanged;
        public Action<string> onDeath;

        private void Start()
        {
            currentHealth = healthValues.MaxHealth;
        }

        #region Change Health

        public void Damage(string playerID, int damage)
        {
            currentHealth -= damage;

            OnHealthChanged?.Invoke();

            CheckHealth(playerID);
        }

        public void Regen(float amount)
            => currentHealth = Mathf.Clamp(currentHealth + amount, 1, healthValues.MaxHealth);

        public void Reset()
        {
            currentHealth = healthValues.MaxHealth;
            OnHealthChanged?.Invoke();
        }    

        #endregion

        public void CheckHealth(string playerID)
        {
            if (currentHealth < 1)
            {
                currentHealth = 0;
                onDeath?.Invoke(playerID);
            }
        }

        public int GetMaxHealth()
            => healthValues.MaxHealth;
    }
}