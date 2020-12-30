using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlaneBodyHit : MonoBehaviour
{
    PlayerController playerController;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void OnCollisionExit(Collision collision)
    {
        Obstacles obstacle = collision.gameObject.GetComponent<Obstacles>();
        if (obstacle != null)
        {
            playerController.IsOutOfControl = true;
        }
    }
}
