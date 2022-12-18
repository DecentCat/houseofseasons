using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform elementOfInterest;
    public float movementSpeed = 3f;
    public float cameraDistance = -20f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(elementOfInterest != null)
        {
            Vector3 pos = new Vector3(elementOfInterest.position.x, elementOfInterest.position.y, cameraDistance);
            transform.position = Vector3.Slerp(transform.position, pos, movementSpeed * Time.deltaTime);
        }
    }
}
