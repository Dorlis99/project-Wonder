using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debug_handsPositiontesting : MonoBehaviour
{
    public GameObject grabbedObject;
    public bool isRight;

    void Start()
    {
        
    }

    void Update()
    {
        if(grabbedObject != null)
        {
            var OH = grabbedObject.GetComponent<objectHandler>();
            OH.Grab("L");
            grabbedObject.transform.position = gameObject.transform.position;
            grabbedObject.transform.rotation = gameObject.transform.rotation;
            grabbedObject.transform.position += OH.holdingCorrectionPosition; //Correct the position offset when holding the object
            grabbedObject.transform.rotation = grabbedObject.transform.rotation * Quaternion.Euler(OH.holdingCorrectionRotation);
            if(isRight)
            {
                grabbedObject.transform.rotation = grabbedObject.transform.rotation * Quaternion.Euler(OH.holdingCorrectionRotationRight);
                OH.Grab("R");

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<objectHandler>()!=null)
        {
            grabbedObject = other.gameObject;
        }
    }
}
