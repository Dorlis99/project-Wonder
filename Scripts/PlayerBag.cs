using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBag : MonoBehaviour
{
    public bool thisIsActiveBag;
    public GameObject[] potionSlots;
    public Mesh bagOpenMesh;
    public Mesh bagClosedMesh;
    public Vector3 bagScaleWhenClosed;
    public Vector3 bagScaleWhenOpen;

    public GameObject closeTrigger;
    public GameObject openTrigger;

    public bool isBagOpen;
    public AudioSource bagSound;
    private MeshFilter myMeshRenderer;
    private GameObject playerBagAnchor;
    //DEBUG ONLY!!! TESTING BAG SAVE WITHOUT VR:
    public bool DEBUG_OpenBagButton;

    void Start()
    {
        myMeshRenderer = gameObject.GetComponent<MeshFilter>();
        closeBag();
        playerBagAnchor = GlobalReferenceDatabase.globalreferencedatabase.PlayerBagAnchor;

        //DEBUG ONLY!!! TESTING BAG SAVE WITHOUT VR:
        openBag();
        //
    }

    void Update()
    {
       if(thisIsActiveBag)
        {
            gameObject.transform.position = playerBagAnchor.transform.position;
            gameObject.transform.rotation = playerBagAnchor.transform.rotation;
        }

        //DEBUG ONLY!!! TESTING BAG SAVE WITHOUT VR:
        if(DEBUG_OpenBagButton)
        {
            DEBUG_OpenBagButton = false;
            openBag();
        }

    }

    public void closeBag()
    {
        myMeshRenderer.mesh = bagClosedMesh;
        gameObject.transform.localScale = bagScaleWhenClosed;
        setSlotsActiveTo(false);
        closeTrigger.SetActive(false);
        openTrigger.SetActive(true);
        isBagOpen = false;
        bagSound.Play();
    }

    public void openBag()
    {
        myMeshRenderer.mesh = bagOpenMesh;
        gameObject.transform.localScale = bagScaleWhenOpen;
        setSlotsActiveTo(true);
        openTrigger.SetActive(false);
        closeTrigger.SetActive(true);
        isBagOpen = true;
        bagSound.Play();


    }

    private void setSlotsActiveTo(bool activeState)
    {
        foreach(GameObject obj in potionSlots)
        {
            obj.SetActive(activeState);
        }
    }
}
