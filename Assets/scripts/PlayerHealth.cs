using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour , IDamageable
{
    public float Health
    {
        set
        {
            if (value < health)
            {
                animator.SetTrigger("hit");

            }

            health = value;

            if (health <= 0)
            {
                animator.SetBool("isAlive", false);
                SFXManager.instance.PlayerDeath();
                col.enabled = false;
                rb.simulated = false;
                player.enabled = false;

            }
        }

        get
        {
            return health;
        }
    }


    [SerializeField] PlayerController player;
    [SerializeField] Animator animator;
    [SerializeField] BoxCollider2D col;
    [SerializeField] Rigidbody2D rb;


    public float health;
    public float maxHealth = 12f;

    public delegate void TakeDamage();
    public static event TakeDamage OnDamage;

    private void Awake()
    {
        health = maxHealth;
    }


    public void MakeUntargetable()
    {
        return;
    }

    

    public void OnHit(float damage, Vector2 knockback)
    {
        Health -= damage;
        SFXManager.instance.PlayerHit();
        rb.AddForce(knockback);
        OnDamage();
    }

    public void OnHit(float damage)
    {
        SFXManager.instance.PlayerHit();
        Health -= damage;
        OnDamage();
    }


}
