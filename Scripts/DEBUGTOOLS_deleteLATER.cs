using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DEBUGTOOLS_deleteLATER : MonoBehaviour
{
   
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F10))
        {
            quickInitialiseScenes();
        }

        if(Input.GetKeyDown(KeyCode.F5))
        {
            //Load the ship scene:
            SceneManager.LoadScene("Ship");
        }

        //Counter for testing one line

       // var allBottles = GameObject.FindObjectsOfType<Bottle>();
       // Debug.Log("Bottles found with method A: " + allBottles.Length);


    }


    public void quickInitialiseScenes()
    {
        SceneManager.LoadScene("StartScene");
    }
}
