using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingEnemyProjectile : MonoBehaviour
{
    public float force;
    public ChallangeArenaScoringSystem scoreSystem;
    public GameObject explosionPSprefab;

    void Start()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * force);
        Destroy(gameObject, 6);
    }

    // Update is called once per frame

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            scoreSystem.totalScore -= 100;
            scoreSystem.updateScore();
        }
        Destroy(gameObject);
        Instantiate(explosionPSprefab, gameObject.transform.position, gameObject.transform.rotation);
    }
}
