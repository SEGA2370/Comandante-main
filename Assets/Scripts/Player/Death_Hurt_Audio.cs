using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHurtAudio : MonoBehaviour
{
    [SerializeField] private HealthPoints playerHealthPoints;

    [SerializeField] private AudioSource hurtAudio;
    [SerializeField] private AudioSource deathAudio;

    private float previousHealth;

    void Start()
    {
        if (playerHealthPoints == null) { Debug.LogWarning("[DeathHurtAudio] No HealthPoints"); return; }
        previousHealth = playerHealthPoints.CurrentHealth;
        playerHealthPoints.HealthChanged.AddListener(OnPlayerHealthChanged);
    }

    void OnDestroy()
    {
        if (playerHealthPoints != null)
            playerHealthPoints.HealthChanged.RemoveListener(OnPlayerHealthChanged);
    }

    void OnPlayerHealthChanged()
    {
        if (playerHealthPoints == null) return;
        if (playerHealthPoints.CurrentHealth <= 0)
        {
            if (deathAudio != null) deathAudio.Play();
        }
        else if (playerHealthPoints.CurrentHealth < previousHealth)
        {
            if (hurtAudio != null) hurtAudio.Play();
        }
        previousHealth = playerHealthPoints.CurrentHealth;
    }
}
