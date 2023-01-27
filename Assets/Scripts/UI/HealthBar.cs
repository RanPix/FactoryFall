using UnityEngine;
using TMPro;

namespace GameBase
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] public Health playerHealth;
        [SerializeField] private Transform healthOnBar;//red thing in healthbar
        private float IDKHowToCallIt;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private TMP_Text HPText;

        private float smoothness = 0.2f;
        private float width;

        private void Start()
        {
            playerHealth.OnHealthChanged += OnHealthChanged;
            IDKHowToCallIt = 1 / playerHealth.maxHealth;
        }

        private void Update()
        {
            //Debug.Log(playerHealth.currentHealth);

            float x = Mathf.Lerp(transform.position.x - width + (width * playerHealth.currentHealth * IDKHowToCallIt), healthOnBar.position.x, smoothness);
            healthOnBar.transform.position = new Vector3(x, healthOnBar.position.y, 0);

        }

        private void OnHealthChanged()
        {
            width = rectTransform.rect.width;

            HPText.text = $"{playerHealth.currentHealth}/{playerHealth.maxHealth}";
        }
    }
}
