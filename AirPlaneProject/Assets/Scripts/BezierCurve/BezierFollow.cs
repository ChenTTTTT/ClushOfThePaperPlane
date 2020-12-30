using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierFollow : MonoBehaviour
{
	public BezierCurve curve;

	[SerializeField] float duration = 3;

	private float progress;

	[SerializeField] float delayBeforeFollowCurve = 0;

	bool isDelayFinished = false;

    private IEnumerator Start()
    {
		yield return new WaitForSeconds(delayBeforeFollowCurve);
		isDelayFinished = true;
	}

    private void Update()
	{
		if (isDelayFinished == false)
			return;

		progress += Time.deltaTime / duration;
		if (progress > 1f)
		{
			progress = 1f;
		}

		transform.position = curve.GetPoint(progress);
	}
}
