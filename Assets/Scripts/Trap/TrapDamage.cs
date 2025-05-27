using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    [SerializeField] private float damage;

   protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            HealthPoints healthPoints = collision.GetComponent<HealthPoints>();
            if (healthPoints != null)
            {
                healthPoints.TakeDamage(damage);
            }
        }
    }
}
