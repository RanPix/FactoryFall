using Mirror;
using UnityEngine;
using System;

namespace GameBase
{
    public class Health : NetworkBehaviour
    {
        [field: SerializeField] public float maxHealth { get; private set; }
        [field: SyncVar] public float gotDamage { get; set; }

        [field: SyncVar]
        public float currentHealth
        {
            get => _currentHealth;
            private set
            {
                _currentHealth = value;
                OnHealthChange?.Invoke();
            }
        }
        private float _currentHealth;
        public Action OnHealthChange;
        public Action onDeath;

        private void Start()
        {
            _currentHealth = maxHealth;
            OnHealthChange += CheckHealth;
        }
        [TargetRpc]
        public void Damage()
        {
            currentHealth -= gotDamage;

            print($"damage: {gotDamage}");
            CheckHealth();

        }
        public void CheckHealth()
        {
            print($"health: {currentHealth}");
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
