using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathAnimation : MonoBehaviour
{
    [SerializeField] private HealthPoints healthPoints;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        healthPoints.HealthChanged.AddListener(DeathAnim); // Subscribe to the healthChanged event
    }

    void OnDestroy()
    {
        if (healthPoints != null)
            healthPoints.HealthChanged.RemoveListener(DeathAnim); // аналогично в других класcах
    }

    public void DeathAnim()
    {
        if (healthPoints.IsDead)
        {
            animator.SetTrigger("Death");
            GetComponent<PlayerMovement>().enabled = false;
        }
        else
        {
            animator.SetTrigger("Hurt");
        }
    }
}
