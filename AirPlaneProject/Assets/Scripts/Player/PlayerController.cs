using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] GameUI ui = null;
    [SerializeField] CutSceneManager cutSceneManager = null;
    [SerializeField] Camera mainCamera = null;
    [SerializeField] Transform targetTransform = null;
    [SerializeField] GameObject planeModel = null;
    [SerializeField] float positiveTurnThreshold = 0.4f;
    [SerializeField] float turnSpeed = 0.01f;
    [SerializeField] [Range(0f, 5f)] float _forwardSpeed = 1f;

    [SerializeField] bool _isControllable = false;
    [SerializeField] bool _isOutOfControl = false;

    float _followSpeedBuff = 0f;

    [Header("Extra Mechanic")]
    [SerializeField] bool isOutOfControlCanBeRecovered = false;

    PlayerCharacter player;
    PlayerAnimation playerAnim;
    CameraOffset cameraOffset;
    Rigidbody rig;
    MeshRenderer planeRend;

    Touch touch;
    Coroutine planeOutOfControlCoroutine;
    Coroutine speedBuffCooldownCoroutine;

    void Awake()
    {
        player = GetComponent<PlayerCharacter>();
        playerAnim = GetComponent<PlayerAnimation>();
        rig = GetComponent<Rigidbody>();
        cameraOffset = mainCamera.GetComponent<CameraOffset>();
        planeRend = planeModel.GetComponent<MeshRenderer>();
    }

	IEnumerator Start () 
	{
        // wait for the kid animation at start to end
        yield return new WaitForSeconds(4.3f);

        // activate player visibility, camera, movability and ui
        planeRend.enabled = true;
        cutSceneManager.UpdateCurrentCutSceneCamera(Enums.CutSceneTypes.Game);
        IsControllable = true;
        ui.ShowGameUIAfterStartAnimation();
    }
	
	void Update ()
	{
		if (_isControllable == false || IsOutOfControl == true)
			return;

        UpdateSideMovements();
    }

    public bool IsControllable
    {
        get { return _isControllable; }
        set
        {
            _isControllable = value;

            if (value == true)
            {
                playerAnim.SetAnimSpeedParameter(_forwardSpeed);
                StartCoroutine(StartForwardMovement());
            }
            else
            {
                playerAnim.SetAnimSpeedParameter(0);
            }
        }
    }

    public float ForwardSpeed
    {
        get { return _forwardSpeed; }
        set
        {
            _forwardSpeed = Mathf.Clamp(value, 0f, 5f);
            playerAnim.SetAnimSpeedParameter(_forwardSpeed);
        }
    }

    public void AddToFollowSpeedBuff(float addAmount)
    {
        _followSpeedBuff = Mathf.Clamp(_followSpeedBuff + addAmount, 0f, 2f);

        // reset speed buff timer
        if (speedBuffCooldownCoroutine != null)
        {
            StopCoroutine(speedBuffCooldownCoroutine);
        }
        StartCoroutine(SpeedBuffCooldown());
    }

    public bool IsOutOfControl
    {
        get { return _isOutOfControl; }
        set 
        { 
            _isOutOfControl = value;
            ui.ActivateOutOfControlText(value);

            // do we want the plane to try and get out of 'out of control' state?
            if (isOutOfControlCanBeRecovered)
            {
                if (value == true)
                {
                    if (planeOutOfControlCoroutine != null)
                    {
                        StopCoroutine(planeOutOfControlCoroutine);
                    }
                    planeOutOfControlCoroutine = StartCoroutine(CenterOutOfControlPlane()); // delay that tries to get out of 'out of control' state
                }
                else
                {
                    if (planeOutOfControlCoroutine != null)
                    {
                        StopCoroutine(planeOutOfControlCoroutine);
                    }
                }
            }
            else
            {
                if (value == true)
                {
                    if (planeOutOfControlCoroutine != null)
                    {
                        StopCoroutine(planeOutOfControlCoroutine);
                    }

                    rig.useGravity = true;
                    planeOutOfControlCoroutine = StartCoroutine(OutOfControlCountdown()); // delay to lose game state
                }
                else
                {
                    if (planeOutOfControlCoroutine != null)
                    {
                        StopCoroutine(planeOutOfControlCoroutine);
                    }
                }
            }
        }
    }

    void UpdateSideMovements()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                planeModel.transform.localPosition = new Vector3(planeModel.transform.localPosition.x, planeModel.transform.localPosition.y, 
                    Mathf.Clamp(planeModel.transform.localPosition.z - touch.deltaPosition.x * (turnSpeed * 4f) * Time.deltaTime, -positiveTurnThreshold, positiveTurnThreshold));
            }

            cameraOffset.UpdateCameraZAxis(planeModel.transform.localPosition.z, positiveTurnThreshold);

            // translate to animations
            if (touch.deltaPosition.x > 0)
            {
                playerAnim.SetAnimSidesMovementParameter(1);
            }
            else if (touch.deltaPosition.x < 0)
            {
                playerAnim.SetAnimSidesMovementParameter(2);
            }
            else
            {
                playerAnim.SetAnimSidesMovementParameter(0);
            }
        }
        else if (Input.GetMouseButton(0))
        {
            float sideMovement = Input.GetAxis("Mouse X");

            planeModel.transform.localPosition = new Vector3(planeModel.transform.localPosition.x, planeModel.transform.localPosition.y, 
                Mathf.Clamp(planeModel.transform.localPosition.z - sideMovement * turnSpeed, -positiveTurnThreshold, positiveTurnThreshold));

            cameraOffset.UpdateCameraZAxis(planeModel.transform.localPosition.z, positiveTurnThreshold);

            // translate to animations
            if (sideMovement > 0)
            {
                playerAnim.SetAnimSidesMovementParameter(1);
            }
            else if (sideMovement < 0)
            {
                playerAnim.SetAnimSidesMovementParameter(2);
            }
            else
            {
                playerAnim.SetAnimSidesMovementParameter(0);
            }
        }
        else
        {
            playerAnim.SetAnimSidesMovementParameter(0);
        }
    }

    IEnumerator StartForwardMovement()
    {
        // keep moving forward if didn't reached the end goal
        while (transform.position != targetTransform.position)
        {
            // only when plane is controlled  move forward
            if (_isControllable)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, Time.deltaTime * (_forwardSpeed + _followSpeedBuff));
            }
            yield return null;
        }

        IsControllable = false;

        // hide plane
        planeRend.enabled = false;

        // move to end cutscene camera
        cutSceneManager.UpdateCurrentCutSceneCamera(Enums.CutSceneTypes.PreEnd);
        cutSceneManager.UpdateCurrentCutSceneAnimator(Enums.CutSceneTypes.PreEnd);

        // win state
        player.PlayerState = Enums.playerStateType.Win;

        // wait for the plane to hit the teacher
        yield return new WaitForSeconds(0.7f);

        // teacher fall animation
        cutSceneManager.UpdateCurrentCutSceneAnimator(Enums.CutSceneTypes.End);
    }

    IEnumerator CenterOutOfControlPlane()
    {
        yield return new WaitForSeconds(2f);

        float time = 0;
        Quaternion startValue = transform.rotation;

        while (time < 1f && player.IsCrashed == false && player.PlayerState == Enums.playerStateType.None)
        {
            transform.rotation = Quaternion.Lerp(startValue, Quaternion.Euler(0, 0, 0), time / 0.5f);
            time += Time.deltaTime;

            // worthless - time to center the plane isn't calculated yet
            if (time > 2f)
            {
                player.PlayerState = Enums.playerStateType.Lose;
            }

            yield return null;
        }

        rig.Sleep();

        // if plane crash while out of control, crash is prioritized
        if (player.IsCrashed == false)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        planeOutOfControlCoroutine = null;
        IsOutOfControl = false;
    }

    IEnumerator OutOfControlCountdown()
    {
        // give time for the plane to spin
        yield return new WaitForSeconds(2.5f);

        if (player.IsCrashed == false)
        {
            player.PlayerState = Enums.playerStateType.Lose;
        }

        planeOutOfControlCoroutine = null;
    }

    IEnumerator SpeedBuffCooldown()
    {
        // delay for the speed buff to wear off
        yield return new WaitForSeconds(3f);
        _followSpeedBuff = 0;
        speedBuffCooldownCoroutine = null;
    }
}
