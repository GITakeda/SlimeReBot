using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class FollowingCamera2D : MonoBehaviour
{
    [SerializeField]
    private Movement2d mov2d;

    [SerializeField]
    [Range(0, 1)]
    public float cameraSpeed = 0.125f;
    

    private void FixedUpdate()
    {
        Vector3 desiredPos = mov2d.GetPosition();
        Vector3 newPos = Vector3.Lerp(transform.position, desiredPos, cameraSpeed);
        newPos.z = transform.position.z;

        this.transform.position = newPos;
    }
}
