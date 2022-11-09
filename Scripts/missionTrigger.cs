using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missionTrigger : MonoBehaviour
{
    public bool isActive;
    private MissionControl theMissionControl;
    public bool oneTimeOnly;


    private void Start()
    {
        theMissionControl = GameObject.FindObjectOfType<MissionControl>();
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(isActive)
        {
            if(other.gameObject.tag == "Player")
            {
                theMissionControl.missionInt += 1;
                isActive = false;
                if(oneTimeOnly)
                {
                    Destroy(gameObject);
                }
            }
        }
    }


}
