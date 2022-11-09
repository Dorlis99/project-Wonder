using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamageDealer : MonoBehaviour
{
    public float myDamage;
    public bool isFriendly;
    public bool isPiercing;


    private void OnCollisionEnter(Collision collision)
    {
        if(isFriendly)
        {
            if(collision.gameObject.GetComponent<EnemyAIController>() != false)
            {
                collision.gameObject.GetComponent<EnemyAIController>().dealDamage(myDamage);
                if(isPiercing == false)
                {
                    DestroyProjectile();
                }
                
            }
        }
    }

    public void DestroyProjectile()
    {
        Destroy(gameObject);
    }

}
