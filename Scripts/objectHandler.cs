using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectHandler : MonoBehaviour
{

    public bool isGrabable;
    public bool isThrowable;
    public bool resetVelocityAfterDrop;
    private Rigidbody myRB;
    [Tooltip("Public scripts: ATTRIBUTE IS FOR READ ONLY!")]
    public bool isGrabbed;
    private bool thrownAlready;
    public MeshCollider myCollider;
    public string whichHandIsHolding;
    public GrabberScript grabberScriptThatGrabbedReference;

    public Vector3 holdingCorrectionPosition;
    public Vector3 holdingCorrectionPositionRight;
    public Vector3 holdingCorrectionRotation;
    public Vector3 holdingCorrectionRotationRight;

    private float cooldownBuffer;
    private bool isOnCooldown;

    public bool isStored;


    private bool rvad_objectWasGrabbed;
    private bool rvad_objectWasReleased;

    void Start()
    {
        myRB = gameObject.GetComponent<Rigidbody>();
       
    }

    void Update()
    {
        pickupCooldown();

        if(resetVelocityAfterDrop)
        {
            resetVelocityOnDropFunctionality();
        }
    }

    public void Grab(string hand)
    {
        if(isGrabbed == false)
        {
            if(isStored)
            {
                unStore();
            }
            whichHandIsHolding = hand;
            //myCollider.enabled = false;
            thrownAlready = false;
            isGrabbed = true;
            myRB.isKinematic = true;
        }
    }

    public void LetGo()
    {
        if (isGrabbed == true)
        {
            whichHandIsHolding = "";
            //myCollider.enabled = true;
            isGrabbed = false;
            myRB.isKinematic = false;
        }
    }

    public void throwMe(Vector3 direction)
    {
        if(thrownAlready == false)
        {
            thrownAlready = true;
            myRB.isKinematic = false; //just in case.
            myRB.AddForce(direction);
        }
        
    }

    public void storeIn(GameObject parentObject)
    {
        if(isOnCooldown)
        {
            return; //If object is on cooldown, stop the function from running.
        }
       // Debug.Log("Store in function online!");
        gameObject.transform.parent = parentObject.transform;
        // Debug.Log("Parent set.");
        if(myRB == null) //This was added, because save system had some issues with the bottom line...
        {
            myRB = gameObject.GetComponent<Rigidbody>();
        }
        myRB.isKinematic = true;
        //Debug.Log("Rigidbody is now kinematic.");
        isGrabable = false;
        isOnCooldown = true;
        if(grabberScriptThatGrabbedReference != null)
        {
            grabberScriptThatGrabbedReference.stopGrabbing(); //If bottle is held, stop grabbing.
           // Debug.Log("Grabbing is stopped.");

        }
        //Debug.LogWarning("Object fully stored in!");

        isStored = true;

    }
    
    public void unStore()
    {

        if (isOnCooldown)
        {
            return; //If object is on cooldown, stop the function from running.
        }

        if(gameObject.transform.parent.gameObject.TryGetComponent(out potionStandScript storageSlot)) //IF the object is stored in potion stand, relief potion stand script from duty.
        {
            storageSlot.unStore();
        }

        gameObject.transform.parent = null;
        myRB.isKinematic = true;
        isStored = false;
       // Debug.Log("Object is fully unstored now.");
    }

    public void pickupCooldown()
    {
        if(isOnCooldown)
        {
            if(cooldownBuffer < 1) //2 seconds of cooldown.
            {
                cooldownBuffer += Time.deltaTime;
            }

            if(cooldownBuffer >= 1)
            {
                isOnCooldown = false;
                cooldownBuffer = 0;
                isGrabable = true;
            }
        }
    }


    private void resetVelocityOnDropFunctionality()
    {
        if(isGrabbed)
        {
            rvad_objectWasGrabbed = true;
            rvad_objectWasReleased = false;
        }

        if(isGrabbed == false && rvad_objectWasGrabbed && rvad_objectWasReleased == false)
        {
            rvad_objectWasGrabbed = false;
            rvad_objectWasReleased = true;
            myRB.velocity = Vector3.zero;
        }
    }
}
