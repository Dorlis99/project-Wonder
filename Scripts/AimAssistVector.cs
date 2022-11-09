using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssistVector : MonoBehaviour
{
    public GameObject myHandToFollow;

    public Vector3 followDirection;
    private Vector3 lastKnownPosition;
    private Vector3 targetPosition;

    
    void Update()
    {
        lastKnownPosition = gameObject.transform.position;

        gameObject.transform.position = myHandToFollow.transform.position;

        followDirection = gameObject.transform.position - lastKnownPosition;

        gameObject.transform.rotation = Quaternion.LookRotation(followDirection);

        Vector3.Normalize(followDirection);
    }
}
