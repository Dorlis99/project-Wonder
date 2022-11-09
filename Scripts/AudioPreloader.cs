using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class AudioPreloader : MonoBehaviour
{
    public List<AudioClip> allAudioClips;

    //public int currentAudioIndex;
   // public AudioClip currentLoadedClip;
    //public AudioClip nextLoadedClip;

    //public string[] audioPaths;


//public async Task changeAudioIndex(int audioPathIndex)
   // {
       // currentAudioIndex = audioPathIndex;

        /*
        Debug.Log("Loading audio...");

        AudioClip sound1 = null;
        AudioClip sound2 = null;

        
       var result1 = await Task.Run(() =>
       {
           sound1 = Resources.Load(audioPaths[currentAudioIndex]) as AudioClip;
           Debug.Log("sound1 is: " + sound1);

           return sound1;
       });

        Debug.Log("Result 1 is: " + result1);

        var result2 = await Task.Run(() =>
        {
            sound2 = Resources.Load(audioPaths[currentAudioIndex + 1]) as AudioClip;

            return sound2;
        });

        Debug.Log("Result 1 is: " + result1);


        currentLoadedClip = result1;
        nextLoadedClip = result2;

        Debug.Log(sound2);
        */

        //Async audio loading decided not to work at all, so I'm not dealing with it. I will just take more RAM for no reason at all.

   // }

   
    
       
    

    

}
