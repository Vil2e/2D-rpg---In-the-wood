using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] bool isGameStart;
    [SerializeField] GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        if (isGameStart && player != null)
        {
            Instantiate(player);
        }

    }

    public void StartGame() 
    {
        int totalSceneAmount = SceneManager.sceneCountInBuildSettings;
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;

        if(nextScene > totalSceneAmount - 1) { return; }

        SceneManager.LoadScene(nextScene);
        
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        EditorApplication.ExitPlaymode();
        Application.Quit();

    }


}
