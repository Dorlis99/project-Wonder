using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderBookController : MonoBehaviour
{
    public int currentlySelectedOrder;
    public GameObject Xselector;

    public int[] ordersPrices;
    public GameObject[] ordersPrefabs;
    public GameObject[] checkboxes;
    public Transform saleSpawnpoint;


    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public void selectOrder(int orderNumber)
    {
        currentlySelectedOrder = orderNumber;
        Xselector.transform.position = checkboxes[orderNumber].transform.position;
        Xselector.transform.rotation = checkboxes[orderNumber].transform.rotation;
    }
}
