using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Brain : MonoBehaviour
{
    public enum NPCtypes { background, buyer}
    public enum NPCstates { disabled, roamingAround, goingForPurchase}
    private NavMeshAgent myNavAgent;
    public Animator myAnimator;

    public bool DEBUG_LogMode;
    public NPCtypes myType;
    public NPCstates myState;

    public int[] roamAroundTimeMinMax;
    private bool waitingForTradeApproach;
    private bool isGoingToTrade;
    private bool isThinkingAboutTrade;
    public int purchasePickTryAmount;


    private bool timerON;
    private float timerTime;
    private float timerBuffer;
    private bool timerFinished;


    private bool destinationSet;
    private bool nowWalking;
    private NPC_navpoint selectedNavpoint;

    private NPC_navpoint playerShopNavpoint;
    private Vector3 tracking_LastPosition;

    private void Start()
    {
        myNavAgent = gameObject.GetComponent<NavMeshAgent>();
        tracking_LastPosition = gameObject.transform.position;

    }


    private void Update()
    {
        if(myType == NPCtypes.buyer)
        {
            timer();
            buyerBrain();
            buyerStateChanger();
            TryGetNavPoints();
            AnimationControl();
        }
    }


    private void buyerStateChanger()
    {
        if(myState == NPCstates.roamingAround)
        {
            if(waitingForTradeApproach == false && isGoingToTrade == false)
            {
                waitingForTradeApproach = true;
                timerTime = Random.Range(roamAroundTimeMinMax[0], roamAroundTimeMinMax[1]);
                timerON = true;
            }
            if(waitingForTradeApproach == true && timerFinished)
            {
                if(timerFinished)
                {
                    waitingForTradeApproach = false;
                    isGoingToTrade = true;
                    myState = NPCstates.goingForPurchase;
                    timerFinished = false;
                    selectedNavpoint.numberOfNPCtargettingThisPoint -= 1;
                    destinationSet = false;


                }
            }
        }

        if(myState == NPCstates.goingForPurchase)
        {
            if(Vector3.Distance(gameObject.transform.position, playerShopNavpoint.transform.position) < 6) //Distance to navpoint
            {
                timerTime = 5;
                isThinkingAboutTrade = true;
                if(timerON == false && isThinkingAboutTrade)
                {
                    timerON = true;
                }
            }
            if(isThinkingAboutTrade && timerFinished)
            {
                timerFinished = false;
                //purchase object.
                purchaseRandomObject();
                isThinkingAboutTrade = false;
                isGoingToTrade = false;
                waitingForTradeApproach = false;
                myState = NPCstates.roamingAround;
            }
        }
    }

    private void buyerBrain()
    {
        if(myState == NPCstates.roamingAround)
        {
            if(destinationSet == false)
            {
                if(selectedNavpoint != null)
                {
                    selectedNavpoint.numberOfNPCtargettingThisPoint -= 1; //reset the NPC point counter before selecting a new one.
                    selectedNavpoint = null;
                }
                var randomPoint = Random.Range(0, GlobalReferenceDatabase.globalreferencedatabase.villageNavigationPointsNPC.Length-1);
                selectedNavpoint = GlobalReferenceDatabase.globalreferencedatabase.villageNavigationPointsNPC[randomPoint];
                if(selectedNavpoint.numberOfNPCtargettingThisPoint > 1)
                {
                    selectedNavpoint = null;
                    return;
                }
                else
                {
                    if(selectedNavpoint.pointName == "PlayerShop")
                    {
                        return; //Abort going to point of it is shop at this point.
                    }
                    selectedNavpoint.numberOfNPCtargettingThisPoint += 1;
                    destinationSet = true;
                    myNavAgent.SetDestination(selectedNavpoint.transform.position);
                }

                if(Vector3.Distance(gameObject.transform.position, selectedNavpoint.transform.position) < 5)
                {
                    destinationSet = false;
                }
            }
        }

        if(myState == NPCstates.goingForPurchase)
        {
            if(destinationSet == false)
            {
                if(playerShopNavpoint.numberOfNPCtargettingThisPoint >1) //no more then two NPC at the counter.
                {
                    resetBuyerState();
                }
                else
                {
                    playerShopNavpoint.numberOfNPCtargettingThisPoint += 1;
                    myNavAgent.SetDestination(playerShopNavpoint.transform.position);
                    destinationSet = true;

                }
            }
        }
    }

    private void timer()
    {
        if(timerON && timerFinished == false)
        {
            if(timerBuffer < timerTime)
            {
                timerBuffer += Time.deltaTime;
            }
            if(timerBuffer >= timerTime)
            {
                timerON = false;
                timerFinished = true;
                timerBuffer = 0;
            }
        }
    }

    public void purchaseRandomObject()
    {
        for(int i = 0; i < purchasePickTryAmount; i++)
        {      
            var tradeController = GlobalReferenceDatabase.globalreferencedatabase.TheTradeController;
            var randomPick = Random.Range(0, tradeController.shopTradeSlots.Length - 1);
            var randomlyPickedSlot = tradeController.shopTradeSlots[randomPick];
            if(randomlyPickedSlot != null)
            {
                if(randomlyPickedSlot.itemIsForSale)
                {
                    var coin = Instantiate(GlobalReferenceDatabase.globalreferencedatabase.coinPrefab, randomlyPickedSlot.coinSpawnPoint.transform.position, randomlyPickedSlot.coinSpawnPoint.transform.rotation);
                    coin.GetComponent<coinScript>().coinValue = randomlyPickedSlot.itemPrice;
                    randomlyPickedSlot.clearSlot();
                    return;
                }
               
            }
        }
        resetBuyerState();//RECENTLY ADDED, NOT TESTED HERE!!!!
    }

    public void TryGetNavPoints()
    {
        if(playerShopNavpoint == null)
        {
            foreach (NPC_navpoint nvp in GlobalReferenceDatabase.globalreferencedatabase.villageNavigationPointsNPC)
            {
                if (nvp.pointName == "PlayerShop")
                {

                    playerShopNavpoint = nvp;
                    return;
                }
            }
        }
    }

    public void resetBuyerState()
    {
        timerFinished = false;
        isThinkingAboutTrade = false;
        isGoingToTrade = false;
        waitingForTradeApproach = false;
        myState = NPCstates.roamingAround;
        timerBuffer = 0;
    }

    public void AnimationControl()
    {
        //Check if is moving:
        //Checking nav agent velocity does not work most of the time. Switching to barebone comparing positions.
        /*
        var isMoving = true;
        var v3 = myNavAgent.velocity;
        var sensitivity = 0.2;
        if(v3.x + v3.y + v3.z >= sensitivity)
        {
            if (DEBUG_LogMode) Debug.Log("is moving!");
            isMoving = true;
        }
        if(v3.x + v3.y + v3.z < sensitivity)
        {
            isMoving = false;
            if (DEBUG_LogMode) Debug.Log("Is not moving");

        }

        

        */
        var isMoving = true;
        var sensitivity = 0.0005;
        //CHANGED FROM: 0.001
        var pos = gameObject.transform.position;
        var posCalc = pos.x + pos.y + pos.z;
        var lastPosCalc = tracking_LastPosition.x + tracking_LastPosition.y + tracking_LastPosition.z;

        if (DEBUG_LogMode) Debug.Log("last position: " + tracking_LastPosition);
        if (DEBUG_LogMode) Debug.Log("Current position: " + pos);
        if (DEBUG_LogMode) Debug.Log("Calculation: " + (posCalc - lastPosCalc));

        if (posCalc - lastPosCalc > sensitivity || posCalc-lastPosCalc < -sensitivity)
        {
            isMoving = true;

        }
        else
        {
            isMoving = false;
        }


        if (isMoving && nowWalking == false)
        {
            nowWalking = true;
            myAnimator.Play("Walk");
            if (DEBUG_LogMode) Debug.Log("Walking!");
        }
        if (isMoving == false && nowWalking == true)
        {
            nowWalking = false;
            myAnimator.Play("Idle");
            if (DEBUG_LogMode) Debug.Log("Not Walking");

        }
        tracking_LastPosition = gameObject.transform.position;

    }
}
