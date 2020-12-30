using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    [SerializeField] Enums.PickupType pickupType = Enums.PickupType.Speed;
    [SerializeField] bool isCanRotate = true;
    [SerializeField] float rotationSpeed = 50f;

    private void Start()
    {
        if (isCanRotate)
        {
            StartCoroutine(Rotating());
        }
    }

    public void DestroyPickup()
    {
        Destroy(gameObject);
    }

    public Enums.PickupType GetPickupType() => pickupType;

    IEnumerator Rotating()
    {
        while (isCanRotate)
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            yield return null;
        }
    }
}
