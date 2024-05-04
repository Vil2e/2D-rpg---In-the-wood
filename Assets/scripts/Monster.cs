using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float Health
    {
        set
        {
            health = value;

        }

        get
        {
            return health;
        }
    }

    RoleState mystate;

    protected bool isAlive = true;

    protected Animator animator;

    protected Rigidbody2D rb;

    protected Collider2D physicsCollider;
    protected DetectionZone detectionZone;
    protected bool isAttacking = true;

    public float knockbackForce = 400f;
    public float Damage { get { return damage; } protected set { damage = value; }}
    [SerializeField] float damage = 1f;
    public float bodyRemainTime = 2f;

    public float knockbackCooldown = .5f;
    public float health = 1;
    public float attackRange = .25f;
 


    protected void Start()
    {
        mystate = new RoleState();
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
        animator.SetTrigger("hit");
        detectionZone.canApproach = false;
        animator.SetBool("isMoving", false);
        rb.AddForce(knockback, ForceMode2D.Impulse);

        Invoke("ReApproach", knockbackCooldown);

        if (health <= 0)
        {
            health = 0;
            animator.SetBool("isAlive", false);
            Defeated();
        }

    }

    public void OnHit(float damage)
    {
        Health -= damage;
        animator.SetBool("isMoving", false);

    }

    public void MakeUntargetable()
    {
        rb.simulated = false;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
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

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();

        if (player != null)
        {
            //玩家離開攻擊範圍 解除攻擊mode
            float distance = Vector3.Distance(collision.transform.position, transform.position);
            if (distance > attackRange)
            {
                isAttacking = false;
                animator.SetBool("isAttack", isAttacking);
            }
        }

    }

    //knockback冷卻過後 再次追擊玩家
    protected void ReApproach()
    {
        detectionZone.canApproach = true;

    }

    protected void DestroyEnemy()
    {
        Destroy(gameObject);
    }

}
