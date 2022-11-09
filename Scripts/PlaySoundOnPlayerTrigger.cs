using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnPlayerTrigger : MonoBehaviour

{
    public AudioClip soundToPlay;
    public bool isArmed;
    private AudioSource myAudioSource;
    public bool DestroyAfterPlayback;
    public float clipTime;

    private void Start()
    {
        myAudioSource = gameObject.GetComponent<AudioSource>();
        gameObject.GetComponent<MeshRenderer>().enabled = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (isArmed)
        {
            if (other.gameObject.tag == "Player")
            {
                //Player detected!
                if(myAudioSource == null)
                {
                    gameObject.AddComponent<AudioSource>();
                    myAudioSource = gameObject.GetComponent<AudioSource>();
                }
                myAudioSource.clip = soundToPlay;
                isArmed = false;
                myAudioSource.Play();
                if(DestroyAfterPlayback)
                {
                    Destroy(gameObject, clipTime);
                }

            }
        }

           
    }


}
