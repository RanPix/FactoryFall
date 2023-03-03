using UnityEngine;
using TMPro;
using Mirror;
using UnityEngine.UI;
using PlayerSettings;

namespace GameBase
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] public Image healthBar;
        [SerializeField] private TMP_Text HPText;
        [SerializeField] private float smoothness;

        private float healthPercent;
        private float difference = 0;

        private Health localPlayerHealth;

        static public Color[] healthBarColors = new Color[7] { Color.red, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow, Color.white };//it would be const if it could be const

        public void UpdateColor()
            => healthBar.color = healthBarColors[Settings.healthBarColor];

        private void Start()
        {
            localPlayerHealth = NetworkClient.localPlayer.gameObject.GetComponent<Health>();
            localPlayerHealth.OnHealthChanged += OnHealthChanged;

            healthPercent = localPlayerHealth.currentHealth;

            UpdateColor();
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

            HPText.text = $"{localPlayerHealth.currentHealth}/{localPlayerHealth.GetMaxHealth()}";
        }

        private float HealthLerp()
            => healthBar.fillAmount -= difference * smoothness * Time.deltaTime;
    }
}
