using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


public class InputManager : MonoBehaviour
{
    public static InputManager inputManager;
    public InputDevice leftController;
    public InputDevice rightController;
    public string checkForConnection;

    //CONTROLS

    public Vector2 leftController_Joystick;
    public Vector2 rightController_Joystick;

    public float leftController_grip;
    public float rightController_grip;
    public bool leftController_gripButton;
    public bool rightController_gripButton;


    public Vector3 leftController_velocity;
    public Vector3 rightController_velocity;

    public bool leftController_primaryButton;
    public bool rightController_primaryButton;

    public bool leftController_Trigger;
    public bool rightController_Trigger;

    public bool leftController_secondaryButton;
    public bool rightController_secondaryButton;

    public bool leftController_menuButton;
    


    public AimAssistVector leftHandAimAssistant;
    public AimAssistVector rightHandAimAssistant;

    public Vector3 leftHandDirection;
    public Vector3 rightHandDirection;

    void Start()
    {
        inputManager = gameObject.GetComponent<InputManager>();
        fetchControllers();
        
    }

    

    private void FixedUpdate()
    {

        leftHandDirection = leftHandAimAssistant.followDirection;
        rightHandDirection = rightHandAimAssistant.followDirection;
        
        



        if (leftController != null) //fetch input for left controller
        {
            leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out leftController_Joystick);
            leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.grip, out leftController_grip);
            leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out leftController_gripButton);
            leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceVelocity, out leftController_velocity);
            leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out leftController_primaryButton);
            leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out leftController_Trigger);
            leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out leftController_secondaryButton);
            leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.menuButton, out leftController_menuButton);



        }


        if (rightController != null) //fetch input for right controller
        {
            rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out rightController_Joystick);
            rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.grip, out rightController_grip);
            rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out rightController_gripButton);
            rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceVelocity, out rightController_velocity);
            rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out rightController_primaryButton);
            rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out rightController_Trigger);
            rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out rightController_secondaryButton);





        }


    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space)) //Press space to reconnect controllers.
        {
            Debug.LogWarning("Controllers reconnected!!");
            fetchControllers();
        }


        var inputFeatures = new List<UnityEngine.XR.InputFeatureUsage>(); //COPIED CODE: DEBUG ONLY!!
        if (leftController.TryGetFeatureUsages(inputFeatures))
        {
            foreach (var feature in inputFeatures)
            {
                if (feature.type == typeof(bool))
                {
                    bool featureValue;
                    if (leftController.TryGetFeatureValue(feature.As<bool>(), out featureValue))
                    {
                    }
                }
            }
        }




    }


    public void fetchControllers()
    {
        //left
        var leftHandedControllers = new List<UnityEngine.XR.InputDevice>();
        var desiredCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Left | UnityEngine.XR.InputDeviceCharacteristics.Controller;
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, leftHandedControllers);

        var VleftController = leftHandedControllers[0];
        leftController = VleftController;

        //right

        var rightHandedControllers = new List<UnityEngine.XR.InputDevice>();
        var desiredCharacteristicsRight = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Right | UnityEngine.XR.InputDeviceCharacteristics.Controller;
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristicsRight, rightHandedControllers);

        var VRightController = rightHandedControllers[0];

        rightController = VRightController;

    }
}
