using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.XR;

public class CharacterMovement : MonoBehaviour
{

    //STATS:
    public float characterSpeed;
    public float characterRotationSpeed;
    public float throwForce; //move to global variables catalouge later

    public Rigidbody myRB;
    public Text debugText;
    public GameObject mainCam;

    public GameObject bag1Anchor;
    public GameObject coinSackAnchor;
    public GameObject referenceToToolSockets;
    public GameObject referenceToLeftHand;

    private bool menuButtonTriggered;
    private bool toolSelectOpen;


    void Start()
    {

        GlobalReferenceDatabase.globalreferencedatabase.PlayerBagAnchor = bag1Anchor;
        GlobalReferenceDatabase.globalreferencedatabase.globalMainPlayerCamera = mainCam;
        if (referenceToToolSockets == null)
        {
            referenceToToolSockets = Instantiate(GlobalReferenceDatabase.globalreferencedatabase.toolsMenuPrefab);
            referenceToToolSockets.SetActive(false);
        }



    }

    void Update()
    {

        toggleToolsMenu();
        Movement();

        if(Input.GetKeyDown(KeyCode.Return)) //DEBUG DEBUG DEBUG DEBUG ONLY
        {
            Debug.LogError(InputManager.inputManager.checkForConnection);
        }

        

        if(toolSelectOpen)
        {
            if(Vector3.Distance(gameObject.transform.position, referenceToToolSockets.transform.position) > 15) //closes the menu after going too far.
            {
                referenceToToolSockets.SetActive(false);
                toolSelectOpen = false;
            }
        }
    }


    private void Movement()
    {

        //MOVEMENT

        /*

        //MOVEMENT RELATIVE TO THE CAMERA (not perfect, needs some fixes later.

        var forwardFromCamera = transform.TransformDirection(mainCam.transform.forward);
       var rightFromCamera = transform.TransformDirection(mainCam.transform.right);



        Vector2 leftJoy; //Stored value of a left stick.

        leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out leftJoy);
        debugText.text = leftJoy.ToString();//DEBUG ONLY: Dispaly joystick value.

        var finalmovement = forwardFromCamera * leftJoy.y + rightFromCamera * leftJoy.x; 
        transform.Translate(finalmovement * characterSpeed * Time.deltaTime);

        */


        
        //MOVEMENT RELATIVE TO THE BODY (works okay, but is a little harder to navigate. 

       // Vector2 leftJoy; //Stored value of a left stick.

        //leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out leftJoy);


        var joyx = InputManager.inputManager.leftController_Joystick.x * characterSpeed * Time.deltaTime; //CHANGED: uses input manager instead of own controller reference.
        var joyy = InputManager.inputManager.leftController_Joystick.y * characterSpeed * Time.deltaTime;

        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        transform.position += right * joyx;
        transform.position += forward * joyy;



        //ROTATION

        var rightJoy = InputManager.inputManager.rightController_Joystick;

        var targetRotation = new Vector3(0, characterRotationSpeed * rightJoy.x, 0);

        myRB.transform.Rotate(targetRotation);

    }

    private void toggleToolsMenu()
    {
        if (InputManager.inputManager.leftController_menuButton)
        {
            if (menuButtonTriggered == false)
            {
                menuButtonTriggered = true;
                if (toolSelectOpen)
                {
                    referenceToToolSockets.SetActive(false);
                    toolSelectOpen = false;
                    menuButtonTriggered = true;
                    return;
                }

                if (toolSelectOpen == false)
                {
                    menuButtonTriggered = true;
                    toolSelectOpen = true;
                    referenceToToolSockets.transform.position = referenceToLeftHand.transform.position;
                    referenceToToolSockets.SetActive(true);
                    referenceToToolSockets.transform.LookAt(GlobalReferenceDatabase.globalreferencedatabase.globalMainPlayerCamera.transform);
                    return;
                }

            }
        }
        if (InputManager.inputManager.leftController_menuButton == false)
        {
            menuButtonTriggered = false;
        }
    }
 


}
