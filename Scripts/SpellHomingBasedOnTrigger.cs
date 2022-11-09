using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellHomingBasedOnTrigger : MonoBehaviour
{

    private bool active;
    public float attractionForce;
    public Rigidbody myRB;
    private GameObject myTarget;

    void Start()
    {
    }

    void Update()
    {
        if(myRB == null)
        {
            Destroy(gameObject);
        }
        if(active && myRB != null)
        {
            myRB.AddForce((myTarget.transform.position - gameObject.transform.position) * attractionForce);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.isTrigger) //This line causes the script to IGNORE triggers.
        {
            return;
        }

        if(other.gameObject.GetComponent<PlayerHomingTarget>() != null)
        {
            if(other.gameObject.GetComponent<PlayerHomingTarget>().attractsProjectiles)
            {
                active = true;
                myTarget = other.gameObject;
            }
        }
    }

    public void ActivateManually(GameObject target)
    {
        active = true;
        myTarget = target;
        attractionForce = attractionForce / 2;
    }
}
