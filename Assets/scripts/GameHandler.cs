using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    void Start()
    {
        PlayerData playerData = new PlayerData();
        playerData.position = new Vector3(1, 5);
        playerData.health = 99;

        string json = JsonUtility.ToJson(playerData);
        Debug.Log(json);

        PlayerData LoadPlayerData = JsonUtility.FromJson<PlayerData>(json);
        Debug.Log("position: " + LoadPlayerData.position);
        Debug.Log("health: " + LoadPlayerData.health);

    }

    void Update()
    {
        
    }

    private class PlayerData
    {
        public Vector3 position;
        public int health;
    }
}
