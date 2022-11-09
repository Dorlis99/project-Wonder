using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderBook_paymentTrigger : MonoBehaviour
{

    public OrderBookController myOrderController;
    public GameObject coinPrefab;
    public int totalCoinsValue;
    public List<GameObject> coinsInTrigger;


    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<coinScript>() != null)
        {
            totalCoinsValue += other.GetComponent<coinScript>().coinValue;
            coinsInTrigger.Add(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<coinScript>() != null)
        {
            totalCoinsValue -= other.GetComponent<coinScript>().coinValue;
            coinsInTrigger.Remove(other.gameObject);
        }
    }


    private void Update()
    {
        if(totalCoinsValue > 0)
        {
            if(myOrderController.currentlySelectedOrder != 0)
            {
                if(totalCoinsValue >= myOrderController.ordersPrices[myOrderController.currentlySelectedOrder])
                {
                    commenceSale();
                }
            }
        }
    }

    public void commenceSale()
    {
        foreach(GameObject obj in coinsInTrigger)
        {
            coinsInTrigger.Remove(obj);
            Destroy(obj);
        }
        totalCoinsValue = 0;

        Instantiate(myOrderController.ordersPrefabs[myOrderController.currentlySelectedOrder], myOrderController.saleSpawnpoint.transform.position, myOrderController.saleSpawnpoint.transform.rotation);
    }
}
