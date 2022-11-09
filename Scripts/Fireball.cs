using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{

    public SpellHomingBasedOnTrigger myHomingScript;
    public HandMagicCaster.spells mySpellType;
    public float myDamage;
    public GameObject myTrailPS;
    private Vector3 scaleToApply;
    public GameObject explosionEffectPrefab;

    void Start()
    {
        scaleToApply = new Vector3(0.0378f, 0.0378f, 0.0378f);
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<PlayerSpellHitTrigger>() != null)
        {
            collision.gameObject.GetComponent<PlayerSpellHitTrigger>().hitTheTrigger(mySpellType);
        }

        
        gameObject.transform.DetachChildren();
        myTrailPS.transform.localScale = scaleToApply; //correction to trail scailing problem.
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Instantiate(explosionEffectPrefab, gameObject.transform.position, gameObject.transform.rotation);
    }
}
