using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orderBook_checkbox : MonoBehaviour
{
    public int myOrderNumber;
    public OrderBookController myBookController;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "ShopObjectSelector")
        {
            myBookController.selectOrder(myOrderNumber);
        }
    }
}
