using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthPoints healthPoints;
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;

    void Start()
    {
        healthPoints.HealthChanged.AddListener(HealtBarUpdate); // Subscribe to the healthChanged event
        UpdateHealthBar(); // Update health bar initially
    }

    void HealtBarUpdate()
    {
        UpdateHealthBar(); // Update health bar when health changes
    }

    private void UpdateHealthBar()
    {
        float totalFillAmount = healthPoints.MaxHealth / 10f; // Calculate total fill amount
        float currentFillAmount = healthPoints.CurrentHealth / 10f; // Calculate current fill amount
        totalHealthBar.fillAmount = totalFillAmount; // Update total health bar fill amount
        currentHealthBar.fillAmount = currentFillAmount; // Update current health bar fill amount
    }
}
