using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatsFunctionality : MonoBehaviour
{
    [SerializeField] RectTransform pauseButtonRectTransform = null;
    [SerializeField] RectTransform coinsPanelRectTransform = null;
    [SerializeField] InputField cameraDistanceInputField = null;
    [SerializeField] InputField planeSpeedInputField = null;
    [SerializeField] InputField addCoinsInputField = null;

    [SerializeField] PlayerCharacter player = null;
    [SerializeField] PlayerController playerController = null;
    [SerializeField] CameraOffset cameraOffset = null;

    HorizontalLayoutGroup coinsPanelHorizontalLayoutGroup;

    private void Awake()
    {
        coinsPanelHorizontalLayoutGroup = coinsPanelRectTransform.GetComponent<HorizontalLayoutGroup>();
    }

    private void Start()
    {
        cameraDistanceInputField.text = cameraOffset.GetDistanceToPlayer().ToString("F2");
        planeSpeedInputField.text = playerController.ForwardSpeed.ToString("F2");
        addCoinsInputField.text = "0";
    }

    public void OnToggleRightHanded(bool _isRightHanded)
    {
        if (_isRightHanded) 
        {
            pauseButtonRectTransform.anchorMin = new Vector2(1, 1);
            pauseButtonRectTransform.anchorMax = new Vector2(1, 1);
            pauseButtonRectTransform.pivot = new Vector2(1, 1);
            coinsPanelRectTransform.anchorMin = new Vector2(0, 1);
            coinsPanelRectTransform.anchorMax = new Vector2(0, 1);
            coinsPanelRectTransform.pivot = new Vector2(0, 1);
            coinsPanelHorizontalLayoutGroup.reverseArrangement = false;
        }
        else 
        {
            pauseButtonRectTransform.anchorMin = new Vector2(0, 1);
            pauseButtonRectTransform.anchorMax = new Vector2(0, 1);
            pauseButtonRectTransform.pivot = new Vector2(0, 1);
            coinsPanelRectTransform.anchorMin = new Vector2(1, 1);
            coinsPanelRectTransform.anchorMax = new Vector2(1, 1);
            coinsPanelRectTransform.pivot = new Vector2(1, 1);
            coinsPanelHorizontalLayoutGroup.reverseArrangement = true;
        }
    }

    public void OnCameraDistanceChange(string _newCameraDistanceString)
    {
        if (cameraOffset == null)
            return;

        float value;
        // check if string is a number
        if (float.TryParse(_newCameraDistanceString, out value))
        {
            // calculate new distance to camera x and y axis
            float x = value - 0.05f;
            float y = (x / 0.1f) * 0.025f + 0.1f;
            cameraOffset.UpdateCameraLocalLocation(-x, y);
        }
        else
        {
            Debug.LogError("Insert a positive number without spaces");
            // return to previously worked distance
            cameraDistanceInputField.text = cameraOffset.GetDistanceToPlayer().ToString("F2");
        }
    }

    public void OnPlaneSpeedChanged(string _newPlaneSpeedString)
    {
        if (playerController == null)
            return;

        float value;
        // check if string is a number
        if (float.TryParse(_newPlaneSpeedString, out value))
        {
            if (value >= 0f && value <= 5f)
            {
                // change plane speed in range of 0f to 5f;
                playerController.ForwardSpeed = value;
            }
            else
            {
                Debug.LogError("Insert a number between 0 to 5 (included)");
                // return to previously worked distance
                planeSpeedInputField.text = playerController.ForwardSpeed.ToString("F2");
            }
        }
        else
        {
            Debug.LogError("Insert a positive number without spaces");
            // return to previously worked distance
            planeSpeedInputField.text = playerController.ForwardSpeed.ToString("F2");
        }
    }

    public void OnAddCoinsButton()
    {
        if (player == null || addCoinsInputField == null)
            return;

        int value;
        // check if string is an int number
        if (int.TryParse(addCoinsInputField.text, out value))
        {
            Debug.Log(value);
            if (value > 0)
            {
                // update player coins
                player.Coins += value;
                addCoinsInputField.text = "0";
            }
            else
            {
                addCoinsInputField.text = "0";
            }
            
        }
        else
        {
            Debug.LogError("Insert a positive number without spaces");
            // return to 0
            addCoinsInputField.text = "0";
        }
    }
}
