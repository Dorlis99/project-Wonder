using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities_magicAnchor : MonoBehaviour
{

    //public bool teleportWhenGrabbed;
    public float timeoutThreshold; //How many seconds will it take before the function triggers
    public float maxDistanceFromAnchor;
    public Vector3 anchor; //Position to come back to.
    private float timerBuffer;

    

    void Update()
    {
        if(timerBuffer < timeoutThreshold)
        {
            timerBuffer += 1 * Time.deltaTime;
        }

        if(timerBuffer >= timeoutThreshold)
        {
            timerBuffer = 0;
            var dist = Vector3.Distance(anchor, gameObject.transform.position);

            if(dist > maxDistanceFromAnchor)
            {
                gameObject.transform.position = anchor;
            }
        }

    }
}
