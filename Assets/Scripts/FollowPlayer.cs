using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3F;
    private Vector3 refVec = new Vector3(0, 0, -6);
    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        refVec = new Vector3(refVec.x, refVec.y, Mathf.Clamp(refVec.z + Input.mouseScrollDelta[1], -15, -5));
        // Define a target position above and behind the target transform
        Vector3 targetPosition = target.TransformPoint(refVec);
        if (targetPosition.y <= 0) targetPosition = new Vector3(targetPosition.x, 0.2f, targetPosition.z);

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // Make it look at bird
        transform.LookAt(target);
    }
}
