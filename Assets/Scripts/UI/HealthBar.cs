using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameBase;

public class HealthBar : MonoBehaviour
{
    [SerializeField] public Health playerHealth;
    [SerializeField] private Transform healthOnBar;//red thing in healthbar
    private float smoothness = 0.01f;
    private int width = 600;
    private void Update()
    {
        //Debug.Log(playerHealth.currentHealth);
        float x = Mathf.Lerp(transform.position.x - width + (width * playerHealth.currentHealth / playerHealth.maxHealth), healthOnBar.position.x, smoothness);
        healthOnBar.transform.position = new Vector3(x, healthOnBar.position.y, 0);
    }
}
