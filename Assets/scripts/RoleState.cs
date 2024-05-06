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

    }

    public class RootRole
    {
        public List<Role> roles = new List<Role>();

    }

    public RootRole rootRole = new RootRole();

    [SerializeField] int monsterIndex;

    private void Awake()
    {
        //讀取resource底下的json檔案
        //注意這邊是使用.text
        string info = Resources.Load<TextAsset>("enemyValue").text;
        
        rootRole = JsonConvert.DeserializeObject<RootRole>(info);
        //依照怪物index擷取需要的json file部分
        //這裡轉回去是用Role
        string jsonFile = JsonConvert.SerializeObject(rootRole.roles[monsterIndex]);

        Role monster = JsonConvert.DeserializeObject<Role>(jsonFile);

    }
}


