using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    [SerializeField] Enums.ObstacleType obstacleType = Enums.ObstacleType.None;
    [SerializeField] float rotationSpeed = 10f;

    void Update()
    {
        switch (obstacleType)
        {
            case Enums.ObstacleType.Rotating:
                transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
                break;
        }
    }
}
