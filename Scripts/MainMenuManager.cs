using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public float timeout;
    private float timebuffer;

    private void Update()
    {
        if (timebuffer < timeout)
        {
            timebuffer += 1 * Time.deltaTime;
        }
        if (timebuffer >= timeout)
        {
            prepareGame();
            timebuffer = 0;
            timeout = 900;

        }

    }

   public void prepareGame()
    {
        SceneManager.LoadScene("Ship");
    }
    
}
