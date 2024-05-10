using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ReadJson : MonoBehaviour
{
    public static ReadJson Instance;

    RootRole rootRole = new RootRole();

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }


        ReadMonsterValue();

    }

    //讀monster數值
    void ReadMonsterValue()
    {
        //讀取resource底下的json檔案
        //注意這邊是使用.text
        string info = Resources.Load<TextAsset>("enemyValue").text;

        //這裡得到的資料是RootRole type 裡面是列出所有的monster value
        rootRole = JsonConvert.DeserializeObject<RootRole>(info);

    }

    //輸入index即可取monster數值
    public Role GetMonsterValue(int monsterIndex)
    {
        Role monster = rootRole.roles[monsterIndex - 1];
        return monster;

    }
}
