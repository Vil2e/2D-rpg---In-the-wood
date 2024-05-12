using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] bool isGameStart;//避免在menu畫面生成player
    [SerializeField] GameObject player;
    [SerializeField] GameObject door;

    [SerializeField] RectTransform fader;

    // Start is called before the first frame update
    void Awake()
    {
        if (isGameStart && player != null)
        {
            Instantiate(player);
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



}
