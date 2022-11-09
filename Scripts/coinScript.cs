using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class coinScript : MonoBehaviour
{
    public int coinValue;
    private objectHandler myOH;
    private Rigidbody myRB;
    public bool isPlayerCoin;
    public coinStorage playerSack;
    private bool coinUnlocked;
    private bool buttonWasPressed;
    public TextMeshPro TMPvalue;

    



    private void Start()
    {
        myOH = gameObject.GetComponent<objectHandler>();
        myRB = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(coinValue==0 && isPlayerCoin)
        {
            myRB.isKinematic = true;
            gameObject.transform.position = playerSack.coinSpawnPosition.position;
        }
        else
        {
            coinUnlocked = true;
        }

        unlockCoin();
        TMPvalue.text = coinValue.ToString();

        if(myOH.isGrabbed == false && coinValue > 0 && isPlayerCoin)
        {
            myRB.velocity = Vector3.zero;
            isPlayerCoin = false;
            playerSack.coinReference = null;
            playerSack = null;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(coinValue !=0)
        {
            if (other.gameObject.GetComponent<coinStorage>() != null)
            {
                if (myOH.isGrabbed == false)
                {
                    var storage = other.gameObject.GetComponent<coinStorage>();

                    storage.totalCoinsCount += coinValue;
                    coinValue = 0;
                    Destroy(gameObject);
                }
            }
        }

        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }


    private void unlockCoin()
    {
        if(coinUnlocked == true && myRB.isKinematic)
        {
            myRB.isKinematic = false;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.GetComponent<coinStorage>() != null)
        {
            if(myOH.isGrabbed)
            {
                var storage = other.gameObject.GetComponent<coinStorage>();
                var hand = myOH.whichHandIsHolding;
                var triggerValue = false;

                if(hand == "L")
                {
                    triggerValue = InputManager.inputManager.leftController_Trigger;
                }
                if(hand == "R")
                {
                    triggerValue = InputManager.inputManager.rightController_Trigger;
                }

                if(triggerValue && buttonWasPressed == false)
                {
                    if(storage.totalCoinsCount > 0)
                    {
                        buttonWasPressed = true;
                        storage.totalCoinsCount -= 1;
                        coinValue += 1;
                    }
                }

                if(triggerValue == false && buttonWasPressed == true)
                {
                    buttonWasPressed = false;
                }
            }

            if(myOH.isGrabbed == false && coinValue > 0)
            {
                var storage = other.gameObject.GetComponent<coinStorage>();

                storage.totalCoinsCount += coinValue;
                coinValue = 0;
                Destroy(gameObject);
            }
        }
    }
}
