using UnityEngine;
using TMPro;
using Player;

namespace GameBase
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] public Health playerHealth;
        [SerializeField] private RectTransform healthOnBar;//green thing in healthbar
        [SerializeField] private float IDKHowToCallIt;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private TMP_Text HPText;

        private float smoothness = 0.2f;
        private float width;

        private void Start()
        {
            playerHealth.GetComponent<GamePlayer>().OnRespawn += OnHealthChanged; 
            playerHealth.OnHealthChanged += OnHealthChanged;
            IDKHowToCallIt = 1 / playerHealth.maxHealth;
        }

        private void Update()
        {
            float x = Mathf.Lerp(transform.position.x - width  + (width * playerHealth.currentHealth / playerHealth.maxHealth), healthOnBar.position.x, smoothness);
            healthOnBar.transform.position = new Vector3(x, healthOnBar.position.y, 0);
        }

        private void OnHealthChanged()
        {
            width = rectTransform.rect.width;

            HPText.text = $"{playerHealth.currentHealth}/{playerHealth.maxHealth}";
        }
    }
}
