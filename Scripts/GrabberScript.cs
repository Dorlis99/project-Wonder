using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;



public class GrabberScript : MonoBehaviour
{

    public bool DEBUG_DEBUGMODE;
    public string controllerSide;
    public CharacterMovement parentMovementScript;
    private GameObject grabableObject;
    private objectHandler grabableObjectOH;
    public GameObject attachedController;
    public GameObject ownHandModel;
    private bool didIjustLetGo;
    private SkinnedMeshRenderer MR_model;
    private bool isGripping;
    private bool gripValue;
    private string whichHandIsHolding;


    //grab action interface
    private GrabActionInterfaceTrigger storedGAI;
    public TextMeshPro MyGAIinfoText;


    void Start()
    {
        //parentMovementScript = gameObject.transform.parent.GetComponent<CharacterMovement>();

        MR_model = ownHandModel.GetComponent<SkinnedMeshRenderer>();
        GAI_clearInfotext();

    }

    void Update()
    {
        gripInputListener();
        grabbing();

        manageHandsModelDisappear();

        InteractionInterfaceOnGrab();

    }



    private void grabbing()
    {



        if (gripValue)
        {
            isGripping = true;
            didIjustLetGo = false;

            
            /*
            if(TryGetComponent(out Cork cork))
            {
                Debug.Log("Component found!");
                if(cork.isCorked)
                {
                    Debug.Log("Cork is uncorked.");
                    cork.unCorkThatCork();
                }
            }
            */
            if (grabableObject != null)
            {
                

                grabableObject.transform.position = gameObject.transform.position;
                grabableObjectOH.Grab(whichHandIsHolding);
                grabableObject.transform.rotation = attachedController.transform.rotation;
               grabableObjectOH.whichHandIsHolding = whichHandIsHolding;


                //IMPORTANT EDIT: All OH objects have separate adjustments for left and right hands.
                if(whichHandIsHolding == "L")
                {
                    grabableObject.transform.position += grabableObjectOH.holdingCorrectionPosition; //Correct the position offset when holding the object
                    grabableObject.transform.rotation = grabableObject.transform.rotation * Quaternion.Euler(grabableObjectOH.holdingCorrectionRotation); //Correct the rotation offset.
                }

                if (whichHandIsHolding == "R")
                {
                    grabableObject.transform.position += grabableObjectOH.holdingCorrectionPositionRight;
                    grabableObject.transform.rotation = grabableObject.transform.rotation * Quaternion.Euler(grabableObjectOH.holdingCorrectionRotationRight);
                }

            }
        }

        if (gripValue == false && grabableObject != null && didIjustLetGo == false)
        {

            stopGrabbing(); //CODE FROM THIS FUNCTION WAS HERE BEFORE BUT IT HAVE BEEN MOVED INTO A SEPARATE COMMAND



        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (grabableObject == null)
        {
            if (other.GetComponent<objectHandler>() != null)
            {
                var OH = other.GetComponent<objectHandler>();

                /* cork script: No longer active.
                if (other.GetComponent<Cork>() != null) //IF the object grabbed is a cork, apply custom logic.
                {
                    var corkScript = other.GetComponent<Cork>();
                    if (corkScript.isCorked) //special functionality for bottle corks ONLY ||  IF is corked, do that:
                    {
                        if (corkScript.myBottle.GetComponent<objectHandler>().isGrabbed)
                        {
                            grabableObject = corkScript.gameObject; //IF bottle is grabbed, the cork can be grabbed. 
                            grabableObjectOH = OH;
                        }
                        if (corkScript.myBottle.GetComponent<objectHandler>().isGrabbed == false) //grab the bottle instead of cork.
                        {

                            grabableObject = corkScript.myBottle;
                            grabableObjectOH = corkScript.myBottle.GetComponent<objectHandler>();
                        }


                        return;

                    }
                }

                */

                if (OH.isGrabable)
                {
                    grabableObject = other.gameObject;
                    grabableObjectOH = OH;
                    OH.grabberScriptThatGrabbedReference = this; //Pass the reference to this script. Would it work?
                }
            }
        }

        if(other.gameObject.TryGetComponent(out GrabActionInterfaceTrigger grabActionInterface))
        {
            storedGAI = grabActionInterface;
        }


    }


    //FOR NOW, I'M COMMENTING THIS OUT!!!

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.tag == "pickableByPlayer")
    //    {
    //        grabableObject = other.gameObject;
    //        fetchOH();

    //        /*
    //        if(other.gameObject.TryGetComponent(out Bottle bottle))
    //        {
    //            if(bottle.GetComponent<objectHandler>().isGrabbed)
    //            {
    //                grabableObjectOH = bottle.cork.GetComponent<objectHandler>();
    //            }
    //        }
    //        */
    //    }
    //}



    private void OnTriggerExit(Collider other)
    {
        if (gripValue == false)
        {
            if (other.TryGetComponent(out objectHandler OH))
            {
                OH.grabberScriptThatGrabbedReference = null; //This line replaces the bottom line, as it presumably caused some errors.
                //grabableObjectOH.grabberScriptThatGrabbedReference = null; //Clear the reference to this script.
                grabableObject = null;
                grabableObjectOH = null;
            }
        }
        if(other.TryGetComponent(out GrabActionInterfaceTrigger GAI)) //reset GAI if object left the trigger.
        {
            storedGAI = null;
            GAI_clearInfotext();
        }
           
    }


    private void fetchOH() //I can't remember what I needed that for... But it is never used anywhere...
    {
        if (grabableObject != null || grabableObjectOH == null)
        {
            grabableObjectOH = grabableObject.GetComponent<objectHandler>();
        }
    }


    private void manageHandsModelDisappear()
    {
        if(DEBUG_DEBUGMODE) //IF IN DEBUG MODE, IGNORE THIS FUNCTION AND RETURN PREMATURELY.
        {
            return;
        }

        if (isGripping && grabableObject != null)
        {
            MR_model.enabled = false;
        }
        else
        {
            MR_model.enabled = true;
        }
    }

    private void gripInputListener()
    {

        if (controllerSide == "L")
        {
            gripValue = InputManager.inputManager.leftController_gripButton;
            whichHandIsHolding = "L";
        }
        if (controllerSide == "R")
        {
            gripValue = InputManager.inputManager.rightController_gripButton;
            whichHandIsHolding = "R";

        }

        if(DEBUG_DEBUGMODE) //OVERWRITES INPUT VALUE, FOR TESTING GRAB POS&ROT on GRABABLE OBJECTS
        {
            gripValue = true;
        }
    }


    public void stopGrabbing()
    {
        //Debug.Log("stop grabbing function is run!");
        isGripping = false;
        didIjustLetGo = true;
        grabableObjectOH.LetGo();
        //Debug.Log("Telling the other object's OH to let go.");

        var throwForce = Vector3.zero;
        var throwForceNEW = Vector3.zero;

       // Debug.Log("Controller side: " + controllerSide);
        if (controllerSide == "L")
        {
            //Debug.Log("Left controller function is run!");
            throwForce = InputManager.inputManager.leftController_velocity;
            throwForceNEW = InputManager.inputManager.leftHandDirection;
        }
        if (controllerSide == "R")
        {
           // Debug.Log("Right controller side function is run!");
            throwForce = InputManager.inputManager.rightController_velocity;
            throwForceNEW = InputManager.inputManager.rightHandDirection;

        }

        throwForce = transform.TransformDirection(throwForce); //Maybe this will work?? JUST TESTING THIS LINE!! Spoiler: it works.

        //grabableObjectOH.throwMe(throwForce * parentMovementScript.throwForce); //OLD line for throwing stuff
        grabableObjectOH.throwMe(throwForceNEW * parentMovementScript.throwForce); //NEW LINE for throwing stuff (works SO MUCH BETTER)
        //Debug.Log("Object was thrown/let go.");

        grabableObject = null;
        grabableObjectOH = null;
        //Debug.LogWarning("Script fully completed!!");
    }

    public void InteractionInterfaceOnGrab()
    {
        //This is an interface allowing for different interactions with the world, such as: Grab a ladder to teleport.

        if(grabableObject == null)
        {
            if(storedGAI != null)
            {
                MyGAIinfoText.text = storedGAI.publicTitle;
                MyGAIinfoText.transform.rotation = Quaternion.LookRotation(MyGAIinfoText.transform.position - GlobalReferenceDatabase.globalreferencedatabase.globalMainPlayerCamera.transform.position); //causes the text to face the camera.


                if (gripValue)
                {
                    MyGAIinfoText.text = "";
                    storedGAI.triggerFunction = true;
                }
            }
        }
    }

    public void GAI_clearInfotext()
    {
        MyGAIinfoText.text = "";
    }

}

