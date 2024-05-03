using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour , IDamageable
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

    [SerializeField] float knockbackForce = 400f;
    [SerializeField] float damage = 1f;
    [SerializeField] float bodyRemainTime = 1f;

    [SerializeField] float knockbackCooldown = 1f;

    [SerializeField] float attackRange = .1f;
    bool isAttacking = true;

    public float health = 1;

    float distance;//用來計算玩家的距離

    private void Start()
    {
        detectionZone = GetComponentInChildren<DetectionZone>();
        animator = GetComponent<Animator>();
        animator.SetBool("isAlive", isAlive);
        rb = GetComponentInChildren<Rigidbody2D>();
        physicsCollider = GetComponent<Collider2D>();

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
        rb.AddForce(knockback, ForceMode2D.Impulse);

        Invoke("ReApproach", knockbackCooldown);

    }

    void IDamageable.OnHit(float damage)
    {
        animator.SetBool("isMoving", false);
        Health -= damage;

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
            Vector3 targetPos = collision.transform.position;
            Vector2 direction = (Vector2)(collision.transform.position - parentPos).normalized;

            distance = Vector3.Distance(targetPos, parentPos);
            Vector2 knockback = direction * knockbackForce;

            //當玩家在攻擊範圍時 進入攻擊mode
            if (distance < attackRange)
            {
                isAttacking = true;
                animator.SetBool("isMoving", false);
                playerHealth.OnHit(damage, knockback);
                animator.SetBool("isAttack", isAttacking);
                playerHealth.OnHit(damage, knockback);


            }

            //collision.SendMessage("OnHit", damage, knockback);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();

        if (player != null)
        {
            //玩家離開攻擊範圍 解除攻擊mode
            distance = Vector3.Distance(collision.transform.position, transform.position);
            if (distance > attackRange)
            {
                isAttacking = false;
                animator.SetBool("isAttack", isAttacking);
            }
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
