using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class coinStorage : MonoBehaviour
{
    public bool belongsToThePlayer;
    public GameObject playerInventoryAnchor;
    public int totalCoinsCount;
    public GameObject coinReference;
    public GameObject coinPrefab;
    public Transform coinSpawnPosition;
    public TextMeshPro TMPvalue;

    private void Update()
    {
        TMPvalue.text = totalCoinsCount.ToString();

        if(coinReference == null)
        {
            coinReference = Instantiate(coinPrefab);
            coinReference.GetComponent<coinScript>().playerSack = this;
            coinReference.GetComponent<coinScript>().isPlayerCoin = true;



        }


        if(belongsToThePlayer)
        {
            gameObject.transform.position = playerInventoryAnchor.transform.position;
            gameObject.transform.rotation = playerInventoryAnchor.transform.rotation;
        }
    }


   

}
