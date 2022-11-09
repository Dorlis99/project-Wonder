using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    private Rigidbody myRB;
    private MeshCollider myCollider;
    private objectHandler myObjectHandler;
    public bool DEBUG_ConstantlyUpdateThePositionOfLocalPosAndRot; //FOR adjusting by hand the positions.
    public Color ingredientColorInPotion;
    public bool colorIsSubtractive;
    public string ingredientNameInPotion;
    public int monetaryValue;

    #region SaveDataVariables
    public string prefabName;

    #endregion


    public Vector3 holdingCorrectionPosition;
    public Vector3 holdingCorrectionRotation;



    void Start()
    {
        myRB = gameObject.GetComponent<Rigidbody>();
        myCollider = gameObject.GetComponent<MeshCollider>();
        myObjectHandler = gameObject.GetComponent<objectHandler>();
    }

    void Update()
    {
        if(DEBUG_ConstantlyUpdateThePositionOfLocalPosAndRot)
        {
            gameObject.transform.localPosition = holdingCorrectionPosition;
            gameObject.transform.localRotation = Quaternion.Euler(holdingCorrectionRotation);
        }
    }


    public void becomeStored()
    {
        if(myRB == null) //again, save system is wonky...
        {
            myRB = gameObject.GetComponent<Rigidbody>();
            myCollider = gameObject.GetComponent<MeshCollider>();
            myObjectHandler = gameObject.GetComponent<objectHandler>();
        }

        myRB.isKinematic = true;
        myCollider.enabled = false; //FOR TESTING, THIS LINE IS DISABLED. It may be causing some bugs with Unity collision detection.
        myObjectHandler.isGrabable = false;
        myObjectHandler.enabled = false;
    }

    public void becomeUnstored()
    {
        myRB.isKinematic = false;
        myCollider.enabled = true; //FOR TESTING, THIS LINE IS DISABLED. It may be causing some bugs with Unity collision detection.
        myObjectHandler.isGrabable = true;
        myObjectHandler.enabled = true;
    }

   
}
