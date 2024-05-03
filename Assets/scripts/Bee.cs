using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour , IDamageable
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
                Defeated();
            }
        }

        get
        {
            return health;
        }
    }

    bool isAlive = true;

    Animator animator;

    Rigidbody2D rb;

    Collider2D physicsCollider;
    DetectionZone detectionZone;

    [SerializeField] float knockbackForce = 100f;
    [SerializeField] float damage = 1f;
    [SerializeField] float bodyRemainTime = 1.5f;

    [SerializeField] float knockbackCooldown = .3f;
    public float health = 3;

    private void Start()
    {
        detectionZone = GetComponentInChildren<DetectionZone>();
        animator = GetComponent<Animator>();
        animator.SetBool("isAlive", isAlive);
        rb = GetComponentInChildren<Rigidbody2D>();
        physicsCollider = GetComponent<Collider2D>();
        //detectionZone = GetComponentInChildren<DetectionZone>();

    }


    public void Defeated()
    {
        SFXManager.instance.DeathSound();

        physicsCollider.enabled = false;
        rb.simulated = false;
        detectionZone.canApproach = false;
        CancelInvoke("ReApproach");

        Invoke("DestroyEnemy", bodyRemainTime);

    }


    public void OnHit(float damage, Vector2 knockback)
    {
        Health -= damage;
        detectionZone.canApproach = false;
        animator.SetBool("isMoving", false);
        rb.AddForce(knockback, ForceMode2D.Impulse);

        Invoke("ReApproach", knockbackCooldown);

    }

    void IDamageable.OnHit(float damage)
    {
        Health -= damage;
        animator.SetBool("isMoving", false);

    }

    public void MakeUntargetable()
    {
        rb.simulated = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            Vector3 parentPos = transform.position;

            Vector2 direction = (Vector2)(collision.transform.position - parentPos).normalized;

            Vector2 knockback = direction * knockbackForce;

            //collision.SendMessage("OnHit", damage, knockback);
            playerHealth.OnHit(damage, knockback);

        }
    }

    //knockback冷卻過後 再次追擊玩家
    private void ReApproach()
    {
        detectionZone.canApproach = true;

    }

    void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
