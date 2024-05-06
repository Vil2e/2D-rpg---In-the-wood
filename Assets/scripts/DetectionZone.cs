using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public string target = "Player";
    Rigidbody2D rb;
    Vector2 direction;
    bool playerDetected;

    Vector3 targetPos;
    Animator animator;
    SpriteRenderer render;
    float moveSpeed = 1f;

    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

    List<GameObject> targetList = new List<GameObject>();

    public bool canApproach = true;//用來處理knockback時的cool down

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        animator = GetComponentInParent<Animator>();
        render = GetComponentInParent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canApproach)
        {
            if (playerDetected && targetPos != Vector3.zero)
            {
                direction = (targetPos - transform.position);
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

                animator.SetFloat("XInput", direction.x);
             
                animator.SetFloat("YInput", direction.y);
                animator.SetBool("isMoving", true);

                if (direction.x > 0)
                {
                    render.flipX = false;
                }
                else if (direction.x < 0)
                {
                    render.flipX = true;
                }
            }

        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.CompareTag(target))
        {
            //用list來處理鎖定的玩家
            targetList.Add(collision.gameObject);
            playerDetected = true;
            targetPos = collision.transform.position;
            
        }
     
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(target))
        {
            
            targetList.Remove(collision.gameObject);
            targetPos = Vector3.zero;
            playerDetected = false;
            direction = Vector2.zero;
            animator.SetBool("isMoving", false);
            animator.SetBool("isAttack", false);
        }
    }


}
