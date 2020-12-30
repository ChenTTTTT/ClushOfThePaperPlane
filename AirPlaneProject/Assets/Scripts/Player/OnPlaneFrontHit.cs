using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlaneFrontHit : MonoBehaviour
{
    PlayerCharacter player;
    PlayerController playerController;

    void Awake()
    {
        player = GetComponentInParent<PlayerCharacter>();
        playerController = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Obstacles obstacle = other.gameObject.GetComponent<Obstacles>();
        Pickups pickup = other.gameObject.GetComponent<Pickups>();
        if (obstacle != null)
        {
            player.IsCrashed = true;
        }
        else if (pickup != null)
        {
            pickup.DestroyPickup();

            switch (pickup.GetPickupType())
            {
                case Enums.PickupType.Speed:
                    playerController.AddToFollowSpeedBuff(0.5f);
                    break;
                case Enums.PickupType.Currency:
                    player.Coins += 1;
                    break;
            }
        }
    }
}
