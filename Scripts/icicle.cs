using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class icicle : MonoBehaviour
{
    public HandMagicCaster.spells mySpellType;
    public float myDamage;//depricated
    public GameObject explosionEffectPrefab;
    public int timeToDestroy;
    private Rigidbody rb;
    private Vector3 localScaleOriginal;
    public MeshCollider myMeshCollider;
    private void Start()
    {
        Destroy(gameObject, timeToDestroy);
        rb = gameObject.GetComponent<Rigidbody>();
        localScaleOriginal = gameObject.transform.localScale;

    }


    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerSpellHitTrigger>() != null)
        {
            collision.gameObject.GetComponent<PlayerSpellHitTrigger>().hitTheTrigger(mySpellType);
        }

        rb.isKinematic = true;
        myMeshCollider.enabled = false;

        gameObject.transform.Translate(gameObject.transform.forward * 0.05f);
        gameObject.transform.parent = collision.gameObject.transform;
        //gameObject.transform.localScale = localScaleOriginal;
        gameObject.transform.position = collision.GetContact(0).point;
        Instantiate(explosionEffectPrefab, gameObject.transform.position, gameObject.transform.rotation);

    }
}
