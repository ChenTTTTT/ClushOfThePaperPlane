using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] Text levelText = null;

    MainMenuUI mainMenu;
    Button levelButton;
    Image buttonImage;

    int levelID;

    private void Awake()
    {
        levelButton = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
    }

    private void Start()
    {
        levelButton.onClick.RemoveAllListeners();
        levelButton.onClick.AddListener(OnLevelButtonClicked);
    }

    public void UpdateLevelButton(MainMenuUI _mainMenu, int _levelID)
    {
        mainMenu = _mainMenu;
        levelID = _levelID;
        levelText.text = _levelID.ToString();
    }

    public void ClearColor()
    {
        buttonImage.color = Color.white;
    }

    void OnLevelButtonClicked()
    {
        mainMenu.OnLevelButtonClickedFromLevelButton(levelID);
        buttonImage.color = new Color32(231, 146, 0, 200);
    }
}
