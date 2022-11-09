using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_spawner : MonoBehaviour
{
    public bool isShooting;
    public float fireInterval;
    public float fireForce;
    public GameObject firePrefab;
    private float fireIntervalBuffer;
    public float lifetime;
    void Start()
    {
        
    }

    void Update()
    {
        
        if(isShooting)
        {
            if(fireIntervalBuffer < fireInterval)
            {
                fireIntervalBuffer += Time.deltaTime;
            }

            if(fireIntervalBuffer >= fireInterval)
            {
                fireIntervalBuffer = 0;

                var spawned = Instantiate(firePrefab, gameObject.transform.position, gameObject.transform.rotation);
                spawned.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * fireForce);
                Destroy(spawned, lifetime);
            }
        }

    }
}
