using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GridLayoutGroup levelsGrid = null;
    [SerializeField] LevelButton levelButtonPrefab = null;
    [SerializeField] Animator ActionsPanelAnimator = null;
    [SerializeField] Text levelNameText = null;

    List<LevelButton> levelButtonsList = new List<LevelButton>();

    int currentChosenSceneID = 0;

    private void Start()
    {
        foreach (GameObject child in levelsGrid.transform)
        {
            Destroy(child.gameObject);
        }

        levelButtonsList.Clear();

        for (int i = 0; i < 3; i++)
        {
            LevelButton lb = Instantiate(levelButtonPrefab, levelsGrid.transform, false);
            lb.UpdateLevelButton(this, i + 1);
            levelButtonsList.Add(lb);
        }
    }

    public void OnPlayButton()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.ActivateChosenScene(1);
        }
    }

    public void OnLevelButtonClickedFromLevelButton(int levelID)
    {
        currentChosenSceneID = levelID;

        for (int i = 0; i < levelButtonsList.Count; i++)
        {
            levelButtonsList[i].ClearColor();
        }

        ActionsPanelAnimator.SetTrigger("OnLevelButton");
        StartCoroutine(ShowLevelTextWithDelay());
    }

    public void PlayCurrentChosenSceneToGameManager()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.ActivateChosenScene(currentChosenSceneID);
        }
    }

    public void HideCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void ShowCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    IEnumerator ShowLevelTextWithDelay()
    {
        yield return new WaitForSeconds(0.15f);
        levelNameText.text = currentChosenSceneID > 0 ? $"The Classroom - Act {currentChosenSceneID}" : "";
    }
}
