using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class ChallangeArenaScoringSystem : MonoBehaviour
{
    public int challangeSelected;
    public GameObject fireChallangeDummyPrefab;
    public GameObject iceChallangeDummyPrefab;
    public TextMeshPro scoreText;

    public TextMeshPro fireballSelectText;
    public TextMeshPro icicleSelectText;

    private bool gameIsRunning;

    public int totalScore;

    public float challangeTime;
    private float timerBuffer;
    private float totalTimeElapsed;
    public float spawnInterval;
    public Vector3 spawnPositionStart;
    public Vector3 spawnPointMaxRandomPositions;
    private ChallangeArenaScoringSystem thisScript;

    //1 = fireball challange
    //2 = icicle challange


    void Start()
    {
        thisScript = gameObject.GetComponent<ChallangeArenaScoringSystem>();
    }

    void Update()
    {
        if(gameIsRunning)
        {
            if(timerBuffer < spawnInterval)
            {
                timerBuffer += Time.deltaTime;
            }
            if(timerBuffer >= spawnInterval)
            {
                timerBuffer = 0;
                spawnDummyInRandom(challangeSelected);

            }

            if(totalTimeElapsed < challangeTime)
            {
                totalTimeElapsed += Time.deltaTime;
            }

            if(totalTimeElapsed >= challangeTime)
            {
                gameIsRunning = false;
                timerBuffer = 0;
                totalTimeElapsed = 0;
                updateScore();
            }
        }


    }

    public void setTextToSomething(string something)
    {
        scoreText.text = something;
    }

    public void changeMode(int mode)
    {
        challangeSelected = mode;

        if(mode == 1)
        {
            fireballSelectText.color = Color.green;
            icicleSelectText.color = Color.red;
        }
        if (mode == 2)
        {
            fireballSelectText.color = Color.red;
            icicleSelectText.color = Color.green;
        }

    }

    public void startChallange()
    {
        if(challangeSelected == 0)
        {
            return;
        }

        if(gameIsRunning)
        {
            return;
        }

        if(gameIsRunning == false)
        {
            gameIsRunning = true;
            timerBuffer = 0;
            totalScore = 0;
        }
    }

    public void updateScore()
    {
        scoreText.text = "Score: " + totalScore.ToString();

        if (gameIsRunning)
        {
            scoreText.color = Color.red;
        }
        if(gameIsRunning == false)
        {
            scoreText.color = Color.green;
        }
    }

    public void spawnDummyInRandom(int mode)
    {
        var r1 = Random.Range(spawnPositionStart.x, spawnPointMaxRandomPositions.x);
        var r2 = Random.Range(spawnPositionStart.y, spawnPointMaxRandomPositions.y);
        var r3 = Random.Range(spawnPositionStart.z, spawnPointMaxRandomPositions.z);

        var spawnPos = new Vector3(r1, r2, r3);

        GameObject dummy = null;

        if(mode == 1)
        {
            dummy = Instantiate(fireChallangeDummyPrefab, spawnPos, fireChallangeDummyPrefab.transform.rotation);
        }
        if(mode == 2)
        {
            dummy = Instantiate(iceChallangeDummyPrefab, spawnPos, fireChallangeDummyPrefab.transform.rotation);
        }

        dummy.GetComponent<DummyArenaScript>().scoreSystem = thisScript;
        
    }
}
