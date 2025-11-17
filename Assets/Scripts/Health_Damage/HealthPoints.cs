using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthPoints : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private UnityEvent _healthChanged;

    public float MaxHealth
    {
        get { return _maxHealth; }
        private set { _maxHealth = value; }
    }

    public UnityEvent HealthChanged => _healthChanged;

    public float CurrentHealth { get; private set; }

    public bool IsDead => CurrentHealth <= 0;

    private bool isAlive = true;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isAlive) // Only apply damage if the player is alive
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);
            HealthChanged.Invoke();

            if (IsDead)
            {
                isAlive = false; // Mark the player as dead
            }
        }
    }

    public void AddHealth(float value)
    {
        if (isAlive) // Only add health if the player is alive
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + value, 0, MaxHealth);
            HealthChanged.Invoke();
        }
    }
    public void ForceRevive(float reviveHealth)
    {
        // Clamp to [0..MaxHealth], mark alive if > 0, and notify listeners
        CurrentHealth = Mathf.Clamp(reviveHealth, 0f, MaxHealth);
        if (CurrentHealth > 0f)
        {
            // mark alive again
            // (your IsDead uses CurrentHealth <= 0; there’s also 'isAlive' flag)
            var isAliveField = typeof(HealthPoints).GetField("isAlive", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (isAliveField != null) isAliveField.SetValue(this, true);
        }
        HealthChanged.Invoke();
    }
}
