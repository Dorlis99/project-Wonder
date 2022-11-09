using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    public bool thisIsALid;
    public ChestScript otherChestScript;
    private Animator myAnimator;

    public GameObject[] checkMarks;
    public string[] potions;


    void Start()
    {
        if(thisIsALid == false)
        {
            myAnimator = gameObject.GetComponent<Animator>();
        }
    }

    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(thisIsALid)
        {
            if(other.gameObject.tag == "Player")
            {
                otherChestScript.openLid();
            }
        }

        if (thisIsALid == false)
        {
            if(other.GetComponent<Bottle>() != null)
            {
                CheckForPotionsInTheChest(other.GetComponent<Bottle>());
                Destroy(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(thisIsALid)
        {
            if (other.gameObject.tag == "Player")
            {
                otherChestScript.closeLid();
            }
        }
    }

    public void openLid()
    {
        myAnimator.Play("Open");
    }

    public void closeLid()
    {
        myAnimator.Play("Close");
    }

    private void CheckForPotionsInTheChest(Bottle bottle)
    {
        var i = 0;
        foreach(GameObject CM in checkMarks)
        {
            if(bottle.liquidFlaskContentName == potions[i])
            {
                CM.SetActive(true);
                return;
            }
            i += 1;
        }
    }
}
