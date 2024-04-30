using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour , IDamageable
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

    [SerializeField] float knockbackForce = 300f;
    [SerializeField] float damage = 1f;
    [SerializeField] float bodyRemainTime = 2f;

    [SerializeField] float knockbackCooldown = 1f;
    public float health = 8;

    [SerializeField] float attackRange = .2f;
    bool isAttacking = true;
    float distance;//用來計算玩家的距離

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
        animator.SetBool("isMoving", false);
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
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
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
                damageable.OnHit(damage, knockback);
                animator.SetBool("isAttack", isAttacking);
                damageable.OnHit(damage,knockback);


            }

            //collision.SendMessage("OnHit", damage, knockback);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
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
