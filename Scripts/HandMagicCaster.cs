using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandMagicCaster : MonoBehaviour
{
    public enum spells { None, Fireball, Icicle}

    public bool shieldActivated;
    public bool myControllerShieldButton;
    public string hand;
    public GameObject controllerReference;
    public Transform myCastDirection;
    public GameObject mainCam;
    public spells spellSelected;
    public GameObject spellChoiceWheelPREFAB;
    private GameObject spellChoiceWheel;
    public float castingTime;
    public float damage;
    public float duration;
    public float spellForce;
    public float spellCorrectionForce;
    private bool wheelOpened;
    private bool buttonPressed;

    private bool myTriggerInput;
    private Vector3 myControllerVelocity;
    private float castBuffer;
    private bool isCasting;
    private GameObject chargePrefabRef;
    private bool spellReady;
    private GameObject lockedOnTarget;
    private int amountCast;
    private float pauseBuffer;

    #region MoveToScriptableObjectLater

    public GameObject myAimAssistant;

    public GameObject fireballPrefab;
    public GameObject fireballChargeParticlePREFAB;

    public GameObject iciclePrefab;
    public GameObject icicleChargeParticlePREFAB;

    
    private bool shieldExists;
    private GameObject shieldReference;
    public GameObject shieldPrefab;
    public GameObject aimParticleSystem;
    private GameObject targetLockOnBuffer;


    #endregion

    public GameObject[] spawnablePrefabList;
    public GameObject[] referencesToHands;


    public UnityEngine.UI.Text DEBUG_DEBUGTEXT0;

    void Start()
    {
    }

    void Update()
    {
        if(myAimAssistant == null)
        {
            var newaim = Instantiate(spawnablePrefabList[0]);
            myAimAssistant = newaim;
            Debug.LogWarning("Setting bullshit #1");

            if (hand == "L")
            {
                myAimAssistant.GetComponent<AimAssistVector>().myHandToFollow = referencesToHands[0];
                InputManager.inputManager.leftHandAimAssistant = myAimAssistant.GetComponent<AimAssistVector>();
            }

            if (hand == "R")
            {
                myAimAssistant.GetComponent<AimAssistVector>().myHandToFollow = referencesToHands[1];
                InputManager.inputManager.rightHandAimAssistant = myAimAssistant.GetComponent<AimAssistVector>();

            }
        }

        if(aimParticleSystem == null)
        {
            Debug.LogWarning("Setting bullshit #2");

            var aimPar = Instantiate(spawnablePrefabList[1]);
            aimParticleSystem = aimPar;
        }


        setMyTriggerInput();

        if (wheelOpened == false)
        {
            if(hand == "L")
            {
                if(InputManager.inputManager.leftController_primaryButton)
                {
                    wheelOpened = true;
                    buttonPressed = true;
                    spellChoiceWheel = Instantiate(spellChoiceWheelPREFAB, controllerReference.transform.position, Quaternion.Euler(0, 0, 0));
                    spellChoiceWheel.transform.LookAt(mainCam.transform);

                }
            }

            if(hand == "R") //COPY OF THE CODE FOR THE OTHER HAND (avoid repetition later?) Actually I can change it now XD but no time
            {
                if (InputManager.inputManager.rightController_primaryButton)
                {
                    wheelOpened = true;
                    buttonPressed = true;
                    spellChoiceWheel = Instantiate(spellChoiceWheelPREFAB, controllerReference.transform.position, Quaternion.Euler(0, 0, 0));
                    spellChoiceWheel.transform.LookAt(mainCam.transform);

                }
            }


            if(shieldActivated)
            {
                if (myControllerShieldButton)
                {
                    if (shieldExists == false)
                    {
                        shieldReference = Instantiate(shieldPrefab);
                        shieldExists = true;
                    }
                    if(shieldExists)
                    {
                        shieldReference.transform.position = myCastDirection.transform.position;

                        shieldReference.transform.rotation = Quaternion.LookRotation(myCastDirection.transform.forward);
                    }
                }

                if(myControllerShieldButton == false)
                {
                    Destroy(shieldReference);
                    shieldReference = null;
                    shieldExists = false;
                }
            }

            PlayerHomingTargetLock();

        }



        if (hand == "L")
        {
            if (buttonPressed && wheelOpened)
            {

                if (InputManager.inputManager.leftController_primaryButton == false)
                {

                    closeTheWheel();
                }
            }
        }

        if (hand == "R")
        {
            if (buttonPressed && wheelOpened)
            {
                if (InputManager.inputManager.rightController_primaryButton == false)
                {
                    closeTheWheel();
                }
            }
        }

        //SPELLS BELOW

        if (spellSelected == spells.None)
        {
            //Well, do nothing. duh.
            return;
        }


        

        if (spellSelected == spells.Fireball)
        {
            selectedSpell_Fireball();

        }

        if (spellSelected == spells.Icicle)
        {
            selectedSpell_Icicle();

        }


    }

    private void selectedSpell_None()
    {

    }

    private void selectedSpell_Fireball()
    {
        castingTime = 0.5f;
        duration = 5.0f;
        damage = 15.0f;
        spellForce = 20000.0f;
        spellCorrectionForce = 3000.0f;
        //Fireball functionality.
        if (myTriggerInput) //Uses own trigger data variable set in a different function of this class.
        {
            if (isCasting == false)
            {
                isCasting = true;
                chargePrefabRef = Instantiate(fireballChargeParticlePREFAB);


            }

        }

        if (isCasting)
        {
            if(chargePrefabRef != null)
            {
                chargePrefabRef.transform.position = gameObject.transform.position;

            }
            if (castBuffer < castingTime)
            {
                castBuffer += Time.deltaTime;
            }

            if (castBuffer >= castingTime)
            {
                spellReady = true;

               
            }
        }

        if(myTriggerInput == false)
        {
            if(spellReady)
            {

                

                //Cast the spell.
                var fireball = Instantiate(fireballPrefab, gameObject.transform.position, gameObject.transform.rotation);
                fireball.transform.rotation = myAimAssistant.transform.rotation;

                var commonAimPoint = Vector3.Lerp(fireball.transform.forward, mainCam.transform.forward, 0.5f); //Middle vector between camera aim and the hand movement

                fireball.GetComponent<Rigidbody>().AddForce(commonAimPoint * spellForce);

                  //HERE IS THE PREVIOUS CODE, FIRING THE FIREBALL IN THE DIRECTION VECTOR OF AN OPEN SIDE OF THE HAND
                //fireball.transform.rotation = myCastDirection.transform.rotation;
                //fireball.GetComponent<Rigidbody>().AddForce(fireball.transform.forward * spellForce);
                spellReady = false;
                Destroy(fireball, 7);//DESTROYS fireball after X seconds.


                if (targetLockOnBuffer != null) //Adds additional force towards the locked-in object, for additional aim assist.
                {
                    var correctionVector = targetLockOnBuffer.transform.position - fireball.transform.position;
                    //fireball.GetComponent<Rigidbody>().AddForce(correctionVector * spellCorrectionForce);
                    fireball.GetComponent<Fireball>().myHomingScript.ActivateManually(targetLockOnBuffer); //Causes the fireball to follow the target when locked on.
                    targetLockOnBuffer = null;
                }

                

            }

            if(spellReady == false)
            {
                isCasting = false;
                castBuffer = 0;
                chargePrefabRef = null;
                targetLockOnBuffer = null;

            }
        }
        
    }


    private void selectedSpell_Icicle()
    {
        castingTime = 2f;
        duration = 7.0f;
        damage = 4.0f;
        spellForce = 8000.0f;
        spellCorrectionForce = 4000.0f;


        if (myTriggerInput) //Uses own trigger data variable set in a different function of this class.
        {
            if (isCasting == false)
            {
                isCasting = true;
                chargePrefabRef = Instantiate(icicleChargeParticlePREFAB);


            }

        }


        if (isCasting)
        {
            if (chargePrefabRef != null)
            {
                chargePrefabRef.transform.position = gameObject.transform.position;

            }
            if (castBuffer < castingTime)
            {
                castBuffer += Time.deltaTime;
            }

            if (castBuffer >= castingTime)
            {
                spellReady = true;
            }
        }

        if(isCasting && spellReady)
        {
            if(amountCast < duration)
            {
                if(pauseBuffer < 0.3f) //float value is a pause between each projectiles being shot.
                {
                    pauseBuffer += Time.deltaTime;
                }

                if(pauseBuffer >= 0.3f) 
                {
                    amountCast += 1;
                    pauseBuffer = 0;
                    var icicle = Instantiate(iciclePrefab, myCastDirection.position, myCastDirection.rotation);
                    icicle.GetComponent<Rigidbody>().AddForce(icicle.transform.forward * spellForce);
                }
                
            }
        }

        if (myTriggerInput == false)
        {
            isCasting = false;
            spellReady = false;
            amountCast = 0;
            castBuffer = 0;
            chargePrefabRef = null;
        }

    }

    private void closeTheWheel()
    {
        wheelOpened = false;
        Destroy(spellChoiceWheel);
        buttonPressed = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(wheelOpened) //When wheel opened, wait for a change of spell
        {
            if(other.GetComponent<SpellcastWheelTag>() != null)
            {
                var tag = other.GetComponent<SpellcastWheelTag>();

                spellSelected = tag.Spell;
            }
        }
    }

    private void setMyTriggerInput()
    {
        if(hand=="L")
        {
            myTriggerInput = InputManager.inputManager.leftController_Trigger;
            myControllerVelocity = InputManager.inputManager.leftController_velocity;
            myControllerShieldButton = InputManager.inputManager.leftController_secondaryButton;
        }
        if(hand=="R")
        {
            myTriggerInput = InputManager.inputManager.rightController_Trigger;
            myControllerVelocity = InputManager.inputManager.rightController_velocity;
            myControllerShieldButton = InputManager.inputManager.rightController_secondaryButton;
        }
    }

    private void PlayerHomingTargetLock()
    {
        if(myTriggerInput)
        {
            if(spellSelected == spells.Fireball)  //ADD ALL COMPATIBILE SPELLS TO IF STATEMENT HERE!
            {

                RaycastHit hit;



                Debug.DrawLine(mainCam.transform.position, mainCam.transform.forward, Color.red);
                if(Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, 4000))
                {
                    DEBUG_DEBUGTEXT0.text = hit.collider.gameObject.name;

                    if (hit.collider.gameObject.GetComponent<PlayerHomingTarget>() != null)
                    {
                        aimParticleSystem.SetActive(true);
                        var pos = hit.collider.gameObject.transform.position;
                        aimParticleSystem.transform.position = new Vector3(pos.x, pos.y + 3, pos.z);
                        lockedOnTarget = hit.collider.gameObject;
                        targetLockOnBuffer = lockedOnTarget;
                    }
                }
                /*
                else
                {
                    DEBUG_DEBUGTEXT0.text = " ";
                    aimParticleSystem.SetActive(false);
                    lockedOnTarget = null;
                }
                */
                //This above code is disabled for now, because I want to try to "leave" the target locked-on even when the camera raycast is taken away. 

            }
        }
        if(myTriggerInput == false) //NOTE: code repeated. Optimize later. (not repeated if the above code will stay commented out.)
        {
            DEBUG_DEBUGTEXT0.text = " ";
            aimParticleSystem.SetActive(false);
            lockedOnTarget = null;
            
        }
    }

}
