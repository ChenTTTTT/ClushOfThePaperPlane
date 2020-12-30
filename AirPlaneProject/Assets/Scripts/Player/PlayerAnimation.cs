using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetAnimSpeedParameter(float speed)
    {
        anim.SetInteger("Speed", Mathf.CeilToInt(speed));
    }

    public void SetAnimSidesMovementParameter(int turnType)
    {
        anim.SetInteger("SidesMovement", Mathf.Clamp(turnType, 0, 2));
    }

    public void SetAnimIsCrashed(bool isCrashed)
    {
        anim.SetBool("IsCrashed", isCrashed);
    }
}
