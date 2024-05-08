using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

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


    public class Role
    {
        public int id;
        public string name;
        public int hp;
        public int damage;
        public int knockback;
        public float knockbackCooldown;
        public float attackRange;
        public float detectionZone;
        public float speed;
        public float bodyRemainTime;

    }

    public class RootRole
    {
        public List<Role> roles = new List<Role>();

    }

    public RootRole rootRole = new RootRole();

    //選取對應的怪物index
    [SerializeField] int monsterIndex;

    protected bool isAlive = true;

    protected Animator animator;

    protected Rigidbody2D rb;

    protected Collider2D physicsCollider;
    protected DetectionZone detectionZone;
    protected float detectRange;
    protected bool isAttacking = true;

    public float Damage { get { return damage; } protected set { damage = value; }}
    [SerializeField] float damage = 1f;

    float bodyRemainTime;
    float knockbackForce;
    float knockbackCooldown;
    float health;
    float attackRange;


    private void Awake()
    {
        //讀取resource底下的json檔案
        //注意這邊是使用.text
        string info = Resources.Load<TextAsset>("enemyValue").text;

        //這裡得到的資料是RootRole type 裡面是列出所有的monster value
        rootRole = JsonConvert.DeserializeObject<RootRole>(info);

        //依照怪物index擷取需要的json file部分
        string jsonFile = JsonConvert.SerializeObject(rootRole.roles[monsterIndex-1]);
        //這裡轉回去是用Role
        Role monster = JsonConvert.DeserializeObject<Role>(jsonFile);


        //賦值
        detectionZone = GetComponentInChildren<DetectionZone>();
        detectRange = GetComponentInChildren<CircleCollider2D>().radius;
        
        Health = monster.hp;
        Damage = monster.damage;
        knockbackForce = monster.knockback;
        knockbackCooldown = monster.knockbackCooldown;
        attackRange = monster.attackRange;
        detectRange = monster.detectionZone;
        detectionZone.MoveSpeed = monster.speed;
        bodyRemainTime = monster.bodyRemainTime;
    }


    protected void Start()
    {
        //mystate = new RoleState();
        
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
