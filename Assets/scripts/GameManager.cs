using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.IO;
using Newtonsoft.Json;


public class GameManager : MonoBehaviour
{
    [SerializeField] bool isGameStart;//避免在menu畫面生成player
    [SerializeField] bool isGamePause;//如果game pause,設定playerControl無反應
    public bool IsGamePause { get { return isGamePause; } }

    [SerializeField] GameObject player;
    [SerializeField] GameObject door;

    [SerializeField] RectTransform fader;
    [SerializeField] GameObject stopMenu;

    string path = Application.dataPath + "/Resources";



    // Start is called before the first frame update
    void Awake()
    {
        if (isGameStart && player != null)
        {
            Instantiate(player);
        }

    }

   

    private void Update()
    {
        if(isGameStart && Input.GetKeyDown(KeyCode.M))
        {
            isGamePause = true;
            stopMenu.SetActive(true);
            Time.timeScale = 0;

        }
        
    }


    public void LoadNextScene() 
    {
        int totalSceneAmount = SceneManager.sceneCountInBuildSettings;
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;

        if(nextScene > totalSceneAmount - 1) { return; }

        fader.gameObject.SetActive(true);
        SceneManager.LoadScene(nextScene);
    }

    public void BackToMenu()
    {
        fader.gameObject.SetActive(true);
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        
    }

    public void QuitGame()
    {
        EditorApplication.ExitPlaymode();
        Application.Quit();

    }

    //用來打開下一關的門
    public void LevelFinished()
    {
        if(door != null)
        {
            door.SetActive(true);
        }

    }

    public void Continue()
    {
        isGamePause = false;
        Time.timeScale = 1;
        stopMenu.SetActive(false);
       
    }

    public void Save()
    {
        SaveData saveData = new SaveData();
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        //寫入目前第幾關
        saveData.levelNumber = currentScene;
        //轉成json、存擋
        string jsonFile = JsonUtility.ToJson(saveData);
        File.WriteAllText(path + "/save.json", jsonFile);
        

    }

    public void Load()
    {

        int level = ReadJson.Instance.GetSavedLevel();

        if (level != 0)
        {
            SceneManager.LoadScene(level);
        }
        else
        {
            print("You dont have any saved file");
        }
    }



}
