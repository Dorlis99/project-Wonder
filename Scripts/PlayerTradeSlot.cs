using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTradeSlot : MonoBehaviour
{
    public bool itemIsForSale;
    public GameObject referenceToObjectInSlot;
    public int itemPrice;
    public PlayerTradeController myController;
    public Transform itemLevitatePosition;
    public Transform coinSpawnPoint;






    

    private void OnTriggerEnter(Collider other)
    {
        //check price:
        if(other.GetComponent<Trade_itemForSale>() != null)
        {
            other.GetComponent<Trade_itemForSale>().reevaluateItemTradeStatus();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(itemIsForSale == false)
        {

           
            
            if (other.gameObject.GetComponent<Trade_itemForSale>() != null)
            {
                if (other.gameObject.GetComponent<objectHandler>().isGrabbed == false)
                {
                    referenceToObjectInSlot = other.gameObject;
                    var item = other.gameObject.GetComponent<Trade_itemForSale>();
                    if (item.thisItemCanBeSold)
                    {
                        itemIsForSale = true;
                        itemPrice = item.value;


                        referenceToObjectInSlot = other.gameObject;
                        referenceToObjectInSlot.GetComponent<Rigidbody>().isKinematic = true;
                        referenceToObjectInSlot.transform.position = itemLevitatePosition.position;
                        referenceToObjectInSlot.transform.rotation = itemLevitatePosition.rotation;


                    }
                }
            }
            
            
            
            
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == referenceToObjectInSlot)
        {
            referenceToObjectInSlot.GetComponent<Rigidbody>().isKinematic = false;
            itemIsForSale = false;
            itemPrice = 0;
            referenceToObjectInSlot = null;
        }
        

    }

    public void clearSlot()
    {
        itemIsForSale = false;
        Destroy(referenceToObjectInSlot);
        referenceToObjectInSlot = null;
        itemPrice = 0;
    }

}
