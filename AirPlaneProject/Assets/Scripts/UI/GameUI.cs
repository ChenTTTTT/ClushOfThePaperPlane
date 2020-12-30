using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] CanvasGroup pauseCanvasGroup = null;
    [SerializeField] CanvasGroup loseCanvasGroup = null;
    [SerializeField] CanvasGroup winCanvasGroup = null;
    [SerializeField] GameObject outOfControlText = null;
    [SerializeField] CanvasGroup coinCanvasGroup = null;
    [SerializeField] Text coinsText = null;

    public void OnPauseButton()
    {
        Time.timeScale = ToggleCanvasGroup(pauseCanvasGroup) == true ? 0 : 1;
    }

    public void OnRestartButton()
    {
        if (GameManager.instance != null)
        {
            Time.timeScale = 1;
            GameManager.instance.RestartGameScene();
        }
    }

    public void OnNextLevelButton()
    {
        if (GameManager.instance != null)
        {
            Time.timeScale = 1;
            GameManager.instance.NextGameScene();
        }
    }

    public void OnMainMenuButton()
    {
        if (GameManager.instance != null)
        {
            Time.timeScale = 1;
            GameManager.instance.ToMainMenuScene();
        }
    }

    public void OpenLoseScreen()
    {
        Time.timeScale = 0;
        ShowCanvasGroup(loseCanvasGroup);
    }

    public void OpenWinScreen()
    {
        Time.timeScale = 0;
        ShowCanvasGroup(winCanvasGroup);
    }

    public void closeLoseAndWinScreens()
    {
        Time.timeScale = 1;
        HideCanvasGroup(loseCanvasGroup);
        HideCanvasGroup(winCanvasGroup);
    }

    public void ShowGameUIAfterStartAnimation()
    {
        ShowCanvasGroup(coinCanvasGroup);
    }

    public void ActivateOutOfControlText(bool _isActive) => outOfControlText.SetActive(_isActive);

    public void OnAddACoinToUI(int newCoinsAmount) => coinsText.text = newCoinsAmount.ToString();

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

    public bool ToggleCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1 - canvasGroup.alpha;
        canvasGroup.interactable = canvasGroup.alpha == 1 ? true : false;
        canvasGroup.blocksRaycasts = canvasGroup.alpha == 1 ? true : false;
        return canvasGroup.alpha == 1 ? true : false;
    }
}
