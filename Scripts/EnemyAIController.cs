using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{
    public enum AIstates { disabled, patrol, wait, chase, attack }
    public AIstates myState;
    private Animator myAnimator;
    private NavMeshAgent myNavAgent;
    public Transform customMiddlePoint;
    private Vector3 spawnLocation;
    public float attackRange;
    public int patrolRange;
    private Vector3 nextPatrolPoint;
    private bool playerDetected;
    private bool wasPatrolPointGenerated;
    private bool isMoving;
    private GameObject referenceToPlayer;
    private bool enemyJustAttacked;
    private bool playerIsChased;

    private float wait_buffer;
    public float wait_waitTime;

    private Vector3 unfreeze_samplePosition;
    private bool unfreeze_wasPositionSampled;
    private float unfreeze_timeoutBuffer;

    public GameObject projectilePrefab;

    public float enemyMaxHP;
    public float enemyCurrentHP;

    public GameObject lootDropPrefab;



    private void Start()
    {
        spawnLocation = gameObject.transform.position;
        myAnimator = gameObject.GetComponent<Animator>();
        myNavAgent = gameObject.GetComponent<NavMeshAgent>();

    }

    private void Update()
    {
        stateSelector();
        behaviourPatterns();
        if(myState != AIstates.disabled)
        {
            unfreezeEnemyRutine();
        }
    }


    private void stateSelector()
    {
        if(myState != AIstates.disabled)
        {
            if (playerDetected && playerIsChased == false)
            {
                playerIsChased = true;
                myState = AIstates.chase;
            }
        }
        
        
       
    }

    private void behaviourPatterns()
    {
        if (myState != AIstates.disabled)//if disabled, this function does not run.
        {
            if (myState == AIstates.patrol)
            {
                if (wasPatrolPointGenerated == false)
                {
                    generateRandomPatrolPoint();
                    wasPatrolPointGenerated = true;

                }
                if(isMoving == false)
                {
                    myNavAgent.SetDestination(nextPatrolPoint);
                    isMoving = true;
                    myAnimator.Play("Move");
                }

                var distanceToPatrolPoint = Vector3.Distance(gameObject.transform.position, nextPatrolPoint);
                if (distanceToPatrolPoint < 3)
                {
                    isMoving = false;
                    //probably wait here for a moment. 
                    myState = AIstates.wait;
                    wasPatrolPointGenerated = false;
                }
            }

            if(myState == AIstates.wait)
            {
                myAnimator.Play("Wait");
                if (wait_buffer < wait_waitTime)
                {
                    wait_buffer += 1 * Time.deltaTime;
                }
                if(wait_buffer >= wait_waitTime)
                {
                    wait_buffer = 0;
                    myState = AIstates.patrol;
                }
            }

            if(myState == AIstates.chase)
            {
                if(referenceToPlayer == null)
                {
                    return;
                }
                myNavAgent.SetDestination(referenceToPlayer.transform.position);

                var distanceToPlayer = Vector3.Distance(gameObject.transform.position, referenceToPlayer.transform.position);
                if(distanceToPlayer < attackRange)
                {
                    myState = AIstates.attack;
                }
            }

            if(myState == AIstates.attack)
            {
                if(enemyJustAttacked == false)
                {
                    myNavAgent.Stop();
                    myAnimator.Play("Attack");
                    enemyJustAttacked = true;
                    //Run attack animation
                }
            }
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            customMiddlePoint.LookAt(other.gameObject.transform);

            RaycastHit hit;

            if(Physics.Raycast(customMiddlePoint.transform.position, customMiddlePoint.transform.forward, out hit, 200))
            {
                if(hit.collider.gameObject.tag == "Player")
                {
                    playerDetected = true;
                    Debug.DrawRay(customMiddlePoint.transform.position, customMiddlePoint.transform.forward * 200, Color.yellow);
                    referenceToPlayer = hit.collider.gameObject;
                }
            }
                


            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerDetected = false;
            referenceToPlayer = null;
        }
    }

    public void generateRandomPatrolPoint()
    {
        for(int i = 0; i < 60; i++)
        {
            Vector3 randomPatrolPoint = spawnLocation + Random.insideUnitSphere * patrolRange;
            NavMeshHit hit;
            if(NavMesh.SamplePosition(randomPatrolPoint, out hit, 100f, NavMesh.AllAreas))
            {
                nextPatrolPoint = hit.position;
                
                return;
            }
        }
        //if the loop fails after 60 trials:
        Debug.LogWarning("generateRandomPatrolPoint() could not generate random patrol point!");
    }

    private void unfreezeEnemyRutine()
    {
        if(myState != AIstates.wait)
        {
            if (unfreeze_wasPositionSampled == false)
            {
                unfreeze_samplePosition = gameObject.transform.position;
                unfreeze_wasPositionSampled = true;
            }

            if (unfreeze_wasPositionSampled)
            {
                var distanceToSample = Vector3.Distance(gameObject.transform.position, unfreeze_samplePosition);
                if (distanceToSample < 2)
                {
                    unfreeze_timeoutBuffer += 1 * Time.deltaTime;
                }
                if (distanceToSample > 2)
                {
                    unfreeze_timeoutBuffer = 0;
                    unfreeze_wasPositionSampled = false;
                }

                if (unfreeze_timeoutBuffer > 5)
                {
                    Debug.LogWarning("Enemy " + gameObject.name + " is stuck! Resetting destination...");
                    wasPatrolPointGenerated = false;
                    isMoving = false;
                    unfreeze_timeoutBuffer = 0;
                }
            }
        }
    }
        
    public void resumeChase()
    {
        enemyJustAttacked = false;
        if(playerDetected)
        {
            myState = AIstates.chase;
        }

        if(playerDetected == false)
        {
            myState = AIstates.patrol;
            playerIsChased = false;
            myNavAgent.SetDestination(nextPatrolPoint);
        }
    }
    
    public void fireProjectile()
    {
        if(referenceToPlayer != null)
        {
            var projectile = Instantiate(projectilePrefab, customMiddlePoint.transform.position, customMiddlePoint.transform.rotation);
            projectile.transform.LookAt(referenceToPlayer.transform);
            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * 1000);
        }
    }

    public void dealDamage(float damage)
    {
        enemyCurrentHP -= damage;

        if(enemyCurrentHP <= 0)
        {
            killEnemy();
        }
    }

    public void killEnemy()
    {
        myState = AIstates.disabled;
        //play dead animation
        myAnimator.Play("Death");
        myNavAgent.isStopped = true;
        Instantiate(lootDropPrefab, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject, 4);
        Destroy(this);
    }
}
