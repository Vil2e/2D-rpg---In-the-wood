using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class RoleState : MonoBehaviour
{
    
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


        public Role(int id, string name, int hp, int damage, int knockback, float knockbackCooldown, float attackRange, float detectionZone, float speed)
        {
            this.id = id;
            this.name = name;
            this.hp = hp;
            this.damage = damage;
            this.knockback = knockback;
            this.knockbackCooldown = knockbackCooldown;
            this.attackRange = attackRange;
            this.detectionZone = detectionZone;
            this.speed = speed;
        }


    }

    public class RootRole
    {
        public List<Role> roles = new List<Role>();

    }

    //建一個儲存怪物數值的字典
    Dictionary<int, Role> enemyValues = new Dictionary<int, Role>();

    
    private void Start()
    {

        //讀取resource底下的json檔案, 並轉換成string
        //注意這邊是使用.text
        string info = Resources.Load<TextAsset>("enemyValue").text;

        RootRole rootRole = JsonConvert.DeserializeObject<RootRole>(info);


        //把enemy數值存到字典中
        for(int i = 0; i < rootRole.roles.Count; i++)
        {
            enemyValues.Add(rootRole.roles[i].id, rootRole.roles[i]);
        }


    }
}


