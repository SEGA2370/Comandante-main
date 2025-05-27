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
        previousHealth = playerHealthPoints.CurrentHealth;
        playerHealthPoints.HealthChanged.AddListener(OnPlayerHealthChanged);
    }

    void OnPlayerHealthChanged()
    {
        if (playerHealthPoints.CurrentHealth <= 0)
        {
            deathAudio.Play();
        }
        // Check if the current health is less than the previous health
        else if (playerHealthPoints.CurrentHealth < previousHealth)
        {
            hurtAudio.Play();
        }
        previousHealth = playerHealthPoints.CurrentHealth;
    }

}
