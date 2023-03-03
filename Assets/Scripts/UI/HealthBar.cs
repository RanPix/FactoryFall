using UnityEngine;
using TMPro;
using Mirror;
using UnityEngine.UI;

namespace GameBase
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image healthBar;
        [SerializeField] private TMP_Text HPText;
        [SerializeField] private float smoothness;

        private float healthPercent;
        private float difference = 0;

        private Health localPlayerHealth;

        private void Start()
        {

            localPlayerHealth = NetworkClient.localPlayer.gameObject.GetComponent<Health>();
            localPlayerHealth.OnHealthChanged += OnHealthChanged;

            healthPercent = localPlayerHealth.currentHealth;


            OnHealthChanged();
        }

        private void Update()
        {
            bool reached = healthPercent < healthBar.fillAmount;

            if (difference < 0 ? !reached : reached)
                HealthLerp();
        }

        private void OnHealthChanged()
        {
            healthPercent = localPlayerHealth.currentHealth / localPlayerHealth.GetMaxHealth();

            difference = healthBar.fillAmount - healthPercent;

            HPText.text = localPlayerHealth.currentHealth.ToString();
        }

        private float HealthLerp()
            => healthBar.fillAmount -= difference * smoothness * Time.deltaTime;
    }
}
