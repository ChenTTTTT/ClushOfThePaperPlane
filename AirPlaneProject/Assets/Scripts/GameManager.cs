using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    List<string> scenesInBuild = new List<string>();
    int currentActiveSceneID = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        scenesInBuild.Clear();

        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            int lastSlash = scenePath.LastIndexOf("/");
            scenesInBuild.Add(scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1));
        }
    }

    public void RestartGameScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextGameScene()
    {
        // check for existed next scene name
        if (scenesInBuild.Contains($"GameScene{currentActiveSceneID + 1}"))
        {
            currentActiveSceneID++;
            SceneManager.LoadScene($"GameScene{currentActiveSceneID}");
        }
        else
        {
            Debug.LogError($"Scene name 'GameScene{currentActiveSceneID + 1}' does not exist, moving you to main menu");
            ToMainMenuScene();
        }
    }

    public void ActivateChosenScene(int sceneNumber)
    {
        // check for existed scene name
        if (scenesInBuild.Contains($"GameScene{sceneNumber}"))
        {
            currentActiveSceneID = sceneNumber;
            SceneManager.LoadScene($"GameScene{sceneNumber}");
        }
        else
        {
            Debug.LogError($"Scene name 'GameScene{sceneNumber}' does not exist");
        }
    }

    public void ToMainMenuScene()
    {
        currentActiveSceneID = 0;
        SceneManager.LoadScene("MainMenuScene");
    }
}
