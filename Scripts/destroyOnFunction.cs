using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyOnFunction : MonoBehaviour
{

    public bool spawnEffect;
    public GameObject effectToSpawn;
    public Transform positionToSpawn;



    public void destroyTarget()
    {
        if(spawnEffect)
        {
            Instantiate(effectToSpawn, positionToSpawn.position, positionToSpawn.rotation);
        }
        Destroy(gameObject);
    }

}
