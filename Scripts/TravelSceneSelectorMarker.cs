using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelSceneSelectorMarker : MonoBehaviour
{
    public TravelSceneSelectorMaster myMaster;
    public string SceneToSelect;
    private bool holdingKnife;
    public AudioSource selectAudio;



    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(holdingKnife == false)
        {
            if (other.gameObject.name == "MapSelectKnife")
            {
                if (other.gameObject.GetComponent<objectHandler>().isGrabbed == false)
                {
                    other.gameObject.GetComponent<MeshCollider>().enabled = false;
                    other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    selectAudio.Play();
                    myMaster.selectedSceneToLoad = SceneToSelect;
                    myMaster.contactMainGate();

                    holdingKnife = true;

                }
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (holdingKnife == true)
        {
            if (other.gameObject.name == "MapSelectKnife")
            {
                if (other.gameObject.GetComponent<objectHandler>().isGrabbed == true)
                {
                    other.gameObject.GetComponent<MeshCollider>().enabled = true;

                    other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    myMaster.selectedSceneToLoad = "";
                    //myMaster.clearMaingate();
                    holdingKnife = false;

                }
            }
        }
    }
}
