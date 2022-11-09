using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class potionStandScript : MonoBehaviour
{
    public bool standOccupied;
    public GameObject referenceToBottle;
    public Vector3 correctionPosition;
    public Vector3 correctionRotation;



    private float cooldownBuffer;
    private bool isOnCooldown;

    void Start()
    {
        
    }

    void Update()
    {
        Cooldown();
        //if(standOccupied) //DEBUG ONLY!!  DEBUG ONLY!!  DEBUG ONLY!!  DEBUG ONLY!!  DEBUG ONLY!!  DEBUG ONLY!!
        //{
        //    referenceToBottle.transform.localPosition = correctionPosition;
        //    referenceToBottle.transform.localRotation = Quaternion.Euler(correctionRotation);
        //}
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isOnCooldown == false)
        {
            if (other.TryGetComponent(out Bottle bottle))
            {
                if(other.TryGetComponent(out objectHandler OH))
                {
                    if(OH.isGrabbed)
                    {
                        return;
                    }
                    if(OH.isStored)
                    {
                        return;
                    }
                }

                //Debug.Log("Bottle detected in the stand!");
                if (standOccupied == false)
                {
                   // Debug.Log("Stand was not occupied, but it is now.");
                    standOccupied = true;
                    referenceToBottle = other.gameObject;
                   // Debug.Log("reference to bottle set to: " + referenceToBottle);
                    referenceToBottle.GetComponent<objectHandler>().storeIn(gameObject);
                   // Debug.Log("Store in function run completely!");
                    referenceToBottle.transform.localPosition = correctionPosition;
                    referenceToBottle.transform.localRotation = Quaternion.Euler(correctionRotation);
                   // Debug.Log("Position and rotation set!");
                    isOnCooldown = true;
                }
            }
        }
        
    }

    public void unStore()
    {
        standOccupied = false;
        referenceToBottle = null;
    }


    public void Cooldown()
    {
        if (isOnCooldown)
        {
            if (cooldownBuffer < 1) //2 seconds of cooldown. (changed to 1)
            {
                cooldownBuffer += Time.deltaTime;
            }

            if (cooldownBuffer >= 1)
            {
                isOnCooldown = false;
                cooldownBuffer = 0;
            }
        }
    }

    public void OverrideStoredItem(GameObject bottle)
    {
        
        
        standOccupied = true;
        referenceToBottle = bottle;
        referenceToBottle.GetComponent<objectHandler>().storeIn(gameObject);
        referenceToBottle.transform.localPosition = correctionPosition;
        referenceToBottle.transform.localRotation = Quaternion.Euler(correctionRotation);
        isOnCooldown = true;
        
    }
}
