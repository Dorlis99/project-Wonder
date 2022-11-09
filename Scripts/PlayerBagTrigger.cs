using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBagTrigger : MonoBehaviour
{
    public bool doesThisTriggerOpensTheBag;
    public PlayerBag playerBag;

    private float cooldownBuffer;
    private bool isOnCooldown;

    void Start()
    {
        
    }

    void Update()
    {
        Cooldown();
    }

    private void OnTriggerStay(Collider other)
    {
        if(isOnCooldown == false)
        {
            if (other.TryGetComponent(out GrabberScript GS))
            {
                if (GS.controllerSide == "L")
                {
                    if (InputManager.inputManager.leftController_Trigger)
                    {
                        toggleBagOpen();
                    }
                }

                if (GS.controllerSide == "R")
                {
                    if (InputManager.inputManager.rightController_Trigger)
                    {
                        toggleBagOpen();
                    }
                }
            }
        }
        
    }

    private void toggleBagOpen()
    {
        isOnCooldown = true;
        if(doesThisTriggerOpensTheBag)
        {
            playerBag.openBag();
        }
        else
        {
            playerBag.closeBag();
        }

    }

    public void Cooldown()
    {
        if (isOnCooldown)
        {
            if (cooldownBuffer < 1) 
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
}
