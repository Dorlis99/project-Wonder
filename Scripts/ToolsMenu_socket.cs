using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsMenu_socket : MonoBehaviour
{
    //public manager manager;

    public GameObject referenceToTool;
    public objectHandler toolOH;
    public bool slotOccuipied;
    public float rotationSpeed;


    private void OnTriggerStay(Collider other)
    {
        if(slotOccuipied == false)
        {
            if(other.gameObject.tag == "PlayerTool")
            {
                var otherOH = other.gameObject.GetComponent<objectHandler>();
                if (otherOH.isGrabbed == false)
                {
                    slotOccuipied = true;
                    toolOH = otherOH;
                    referenceToTool = other.gameObject;
                    referenceToTool.transform.parent = gameObject.transform;
                    referenceToTool.GetComponent<Rigidbody>().isKinematic = true;
                    referenceToTool.transform.position = gameObject.transform.position;
                }

            }
        }

        if(slotOccuipied)
        {
            if(toolOH.isGrabbed)
            {
                detachObject();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {/*
        if(slotOccuipied)
        {
            if(other.gameObject == referenceToTool)
            {
                detachObject();
            }
        }
        */
    }


    public void detachObject()
    {
        slotOccuipied = false;
        var worldPosOfObject = referenceToTool.transform.position;
        referenceToTool.transform.SetParent(null, true);
        var refRB = referenceToTool.GetComponent<Rigidbody>();
        refRB.velocity = Vector3.zero;
        referenceToTool.transform.position = worldPosOfObject;
        referenceToTool.GetComponent<Rigidbody>().isKinematic = false;
        referenceToTool = null;
        toolOH = null;
    }
    private void Update()
    {
        if(slotOccuipied)
        {
            referenceToTool.transform.rotation = Quaternion.Euler(referenceToTool.transform.eulerAngles.x, referenceToTool.transform.eulerAngles.y + rotationSpeed, referenceToTool.transform.eulerAngles.z);
        }
    }

}
