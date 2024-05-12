using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] bool isGameStart;
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

    private void Start()
    {
        fader.gameObject.SetActive(true);

        LeanTween.alpha(fader, 1, 0);
        LeanTween.alpha(fader, 0, 0.5f).setOnComplete(() => { fader.gameObject.SetActive(false); });
    }

    public void LoadNextScene() 
    {
        int totalSceneAmount = SceneManager.sceneCountInBuildSettings;
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;

        if(nextScene > totalSceneAmount - 1) { return; }

        fader.gameObject.SetActive(true);

        LeanTween.alpha(fader, 0, 0);
        LeanTween.alpha(fader, 1, 0.5f).setOnComplete(() => { SceneManager.LoadScene(nextScene); });

    }

    public void BackToMenu()
    {
        fader.gameObject.SetActive(true);

        LeanTween.alpha(fader, 0, 0);
        LeanTween.alpha(fader, 1, 0.5f).setOnComplete(() => { SceneManager.LoadScene(0); });

        
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
