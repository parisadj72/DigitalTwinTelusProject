using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtUser : MonoBehaviour
{
    void Update()
    {
        // Get the direction to the camera
        Vector3 directionToCamera = Camera.main.transform.position - transform.position;

        // Set the y component of the direction to zero
        directionToCamera.y = 0;

        // Get the rotation to face the camera
        Quaternion targetRotation = Quaternion.LookRotation(-directionToCamera);

        // Rotate the object to face the camera along the Y-axis
        transform.rotation = targetRotation;
    }
}
