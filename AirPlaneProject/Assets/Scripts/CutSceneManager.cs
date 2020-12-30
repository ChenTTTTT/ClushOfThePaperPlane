using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    [SerializeField] Camera startCutSceneCamera = null;
    [SerializeField] Camera playerCamera = null;
    [SerializeField] Camera endCutSceneCamera = null;

    [SerializeField] Animator startCutSceneAnimator = null;
    [SerializeField] Animator startAirplaneCutSceneAnimator = null;
    [SerializeField] Animator endAirplaneCutSceneAnimator = null;
    [SerializeField] Animator endCutSceneAnimator = null;
    [SerializeField] Animator endCameraAnimator = null;

    [SerializeField] ParticleSystemRenderer teacherStarsParticleRend = null;

    private IEnumerator Start()
    {
        UpdateCurrentCutSceneCamera(Enums.CutSceneTypes.Start);
        yield return new WaitForSeconds(2f);
        UpdateCurrentCutSceneAnimator(Enums.CutSceneTypes.Start);
    }

    public void UpdateCurrentCutSceneCamera(Enums.CutSceneTypes _cutSceneType)
    {
        switch (_cutSceneType)
        {
            case Enums.CutSceneTypes.Start:
                startCutSceneCamera.enabled = true;
                playerCamera.enabled = false;
                endCutSceneCamera.enabled = false;
                break;
            case Enums.CutSceneTypes.Game:
                playerCamera.enabled = true;
                startCutSceneCamera.enabled = false;
                endCutSceneCamera.enabled = false;
                break;
            case Enums.CutSceneTypes.PreEnd:
                endCutSceneCamera.enabled = true;
                startCutSceneCamera.enabled = false;
                playerCamera.enabled = false;
                endCameraAnimator.SetTrigger("CanStart");
                break;
        }
    }
    public void UpdateCurrentCutSceneAnimator(Enums.CutSceneTypes _cutSceneType)
    {
        switch (_cutSceneType)
        {
            case Enums.CutSceneTypes.Start:
                startCutSceneAnimator.SetTrigger("CanStart");
                StartCoroutine(StartAirPlaneCountdown());
                break;
            case Enums.CutSceneTypes.PreEnd:
                endAirplaneCutSceneAnimator.gameObject.SetActive(true);
                endAirplaneCutSceneAnimator.SetTrigger("CanStart");
                break;
            case Enums.CutSceneTypes.End:
                endCutSceneAnimator.SetTrigger("CanStart");
                teacherStarsParticleRend.enabled = true;
                break;
        }
    }

    IEnumerator StartAirPlaneCountdown()
    {
        yield return new WaitForSeconds(0.3f);
        startAirplaneCutSceneAnimator.SetTrigger("CanStart");
        yield return new WaitForSeconds(1.7f);
        startAirplaneCutSceneAnimator.gameObject.SetActive(false);
    }
}
