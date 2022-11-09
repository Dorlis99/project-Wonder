using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ignoreCollisionsToolpatch : MonoBehaviour
{
    public bool ignoreByLayer;
    public bool ignoreByTag;
    public bool ignoreByString;

    public int ignoreFromLayer;
    public int ignoreToLayer;

    public string ignoreTag;

    public string ignoreString;

    public Collider colliderToUse;

    void Start()
    {

        if(ignoreByLayer)
        {
            Physics.IgnoreLayerCollision(ignoreFromLayer, ignoreToLayer);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(ignoreByTag)
        {
            if(collision.gameObject.tag == ignoreTag)
            {
                Physics.IgnoreCollision(colliderToUse, collision.collider);
            }
        }
    }

}
