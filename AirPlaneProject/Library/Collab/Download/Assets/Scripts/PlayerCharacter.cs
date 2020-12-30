using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] GameUI UI = null;
    [SerializeField] bool _isCrashed = false;
    [SerializeField] int _coins = 0;

    PlayerController playerController;
    PlayerAnimation playerAnim;

    Enums.playerStateType _playerState = Enums.playerStateType.None;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerAnim = GetComponent<PlayerAnimation>();
    }

    public bool IsCrashed
    {
        get { return _isCrashed; }
        set 
        { 
            _isCrashed = value;
            playerAnim.SetAnimIsCrashed(value);
            playerController.IsControllable = !value;
            Debug.Log($"Is Crashed: {value}");
            if (value == true)
            {
                _playerState = Enums.playerStateType.Lose;
                StartCoroutine(CrashDelay());
            }
        }
    }

    public int Coins
    {
        get { return _coins; }
        set
        {
            _coins += value;
            UI.OnAddACoinToUI(_coins);
        }
    }

    public Enums.playerStateType PlayerState
    {
        get { return _playerState; }
        set
        {
            switch (value)
            {
                case Enums.playerStateType.None:
                    UI.closeLoseAndWinScreens();
                    break;
                case Enums.playerStateType.Win:
                    UI.OpenWinScreen();
                    break;
                case Enums.playerStateType.Lose:
                    UI.OpenLoseScreen();
                    break;

            }
        }
    }

    IEnumerator CrashDelay()
    {
        yield return new WaitForSeconds(1.5f);
        PlayerState = Enums.playerStateType.Lose;
    }
}
