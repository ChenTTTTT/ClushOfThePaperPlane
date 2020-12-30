using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOffset : MonoBehaviour
{
    [SerializeField] GameObject AirPlaneModel = null;

    float distanceToPlayer = 0;

    private void Start()
    {
        distanceToPlayer = GetDistanceToPlayer();
    }

    public void UpdateCameraLocalLocation(float forward, float upward)
    {
        transform.localPosition = new Vector3(forward, upward, transform.localPosition.z);
        distanceToPlayer = GetDistanceToPlayer();
    }

    // camera Z movement only if camera is close enough
    public void UpdateCameraZAxis(float newZ, float turnThreshold)
    {
        // check if the distance camera to player minus the camera offset threshold is lower then maximum distance
        if (distanceToPlayer - turnThreshold < 0.9f)
        {
            // calculate camera Z movement by camera threshold and camera distance
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Clamp(newZ, -(turnThreshold / (distanceToPlayer * 2)), turnThreshold / (distanceToPlayer * 2)));
        }
    }

    public float GetDistanceToPlayer() => Vector3.Distance(transform.localPosition, new Vector3(AirPlaneModel.transform.localPosition.x, 0, 0));
}
