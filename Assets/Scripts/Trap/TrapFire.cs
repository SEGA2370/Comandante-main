using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFire : TrapDamage
{
    private Animator animator;

    private bool isWorking;

    [SerializeField] private float cooldown;
    private float cooldownTimer;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (isWorking)
        {
            base.OnTriggerEnter2D(collision);
        }
 
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer < 0) 
        { 
         isWorking = !isWorking;
         cooldownTimer = cooldown;
        }

        animator.SetBool("isWorking", isWorking);
    }

}
