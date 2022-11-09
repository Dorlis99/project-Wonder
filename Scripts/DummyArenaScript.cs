using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyArenaScript : MonoBehaviour
{
    public ChallangeArenaScoringSystem scoreSystem;

    public int points;
    public int scoreMultiplyer;
    public int pointsTimeDegradeFactor;
    public int pointsDegradeAmount;
    public int pointsMinumum;
    public float timeToDestroy;
    public float chanceToSpawnProjectile;
    public int howManyProjectilesCanSpawn;
    public GameObject projectileToSpawn;
    public Transform projectileSpawnPoint;

    private float timerBuffer;


    void Start()
    {
        Destroy(gameObject, timeToDestroy); 
    }

    void Update()
    {
        if(points > pointsMinumum)
        {
            if (timerBuffer < pointsTimeDegradeFactor)
            {
                timerBuffer += Time.deltaTime;
            }
            if (timerBuffer >= pointsTimeDegradeFactor)
            {
                points -= pointsDegradeAmount;
                timerBuffer = 0;
                var probability = Random.Range(0, 100);
                if(probability <= chanceToSpawnProjectile)
                {
                    if(howManyProjectilesCanSpawn > 0)
                    {
                        howManyProjectilesCanSpawn -= 1;
                        var proj = Instantiate(projectileToSpawn, projectileSpawnPoint.position, projectileToSpawn.transform.rotation);
                        proj.transform.LookAt(GlobalReferenceDatabase.globalreferencedatabase.PlayerEnemyProjectileTarget);
                        proj.GetComponent<TrainingEnemyProjectile>().scoreSystem = scoreSystem;
                    }
                }
            }
        }

        
    }

    public void destroyTheDummy()
    {
        scoreSystem.totalScore += points * scoreMultiplyer;
        scoreSystem.updateScore();
        Destroy(gameObject);
    }

    public void destroyTheDummyIce()
    {
        scoreSystem.totalScore += points * scoreMultiplyer;
        scoreSystem.updateScore();
        Destroy(gameObject, 0.4f);
    }
}
