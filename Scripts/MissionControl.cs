using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionControl : MonoBehaviour
{
    private string currentScene;
    private AudioPreloader myAudioPreloader;
    private AudioSource globalAudioSource;
    public string missionName;
    public int missionInt;
    private Animator MissionAnimator;
    public int counter1;

    private bool timerActive;
    private float timerBuffer;
    private float timerTime;
    private bool timerProgressMission;

    //NOTE: Tutorial elements are supposed to be filled from a list FROM A LOCAL STORAGE in the given scene. 
    [System.NonSerialized]
    public List<GameObject> Tutorial1_missionElements_Ship;
    [System.NonSerialized]
    public List<GameObject> Tutorial1_missionElements_village;

    private void Start()
    {
        myAudioPreloader = gameObject.GetComponent<AudioPreloader>();
        MissionAnimator = gameObject.GetComponent<Animator>();
        globalAudioSource = gameObject.GetComponent<AudioSource>();
        GlobalReferenceDatabase.globalreferencedatabase.theMissionControl = this;

    }

    void Update()
    {
        missionTimer();

        //RUN on every scene change
        //I'm really sorry, but there is no other way that won't take me ages to code and research now:
        if(SceneManager.GetActiveScene().name != currentScene)
        {
            currentScene = SceneManager.GetActiveScene().name;
            OnEverySceneChange();
        }

        #region Tutorial 1

        if(missionName == "Tutorial1" && missionInt == 0)
        {
            missionInt += 1;
            MissionAnimator.Play("Tutorial1_0");

        }

        if (missionName == "Tutorial1" && missionInt == 2) //After player triggers the doors.
        {
            missionInt += 1;
            MissionAnimator.Play("Tutorial1_1");
            Tutorial1_functions_missionInt2();
        }

        if (missionName == "Tutorial1" && missionInt == 4) //After arriving in the village.
        {
            missionInt += 1;
            Tutorial1_missionElements_village[0].SetActive(true);
            
        }

        if (missionName == "Tutorial1" && missionInt == 6) //After opening the chest
        {
            missionInt += 1;
            Tutorial1_functions_missionInt6();
            //enable guardain chest open animation
            //enable special effects
            //run guardian wake up animation

        }
        if (missionName == "Tutorial1" && missionInt == 8) //After finishing wake up animation
        {
            missionInt += 1;

            //Run sound
            PlayGlobalSoundOfIndex(1);

        }

        if (missionName == "Tutorial1" && missionInt == 9) //Constantly checks if there are 5 bottles in the world. 
        {
            var allBottles = GameObject.FindObjectsOfType<Bottle>();

            if (allBottles.Length == 5)
            {
                missionInt = 10;
            }


        }

        if (missionName == "Tutorial1" && missionInt == 10)
        {
            missionInt += 1;
            //play congratulation sounds
            PlayGlobalSoundOfIndex(4);
            Tutorial1_missionElements_village[2].GetComponent<missionTrigger>().isActive = true;

        }

        if (missionName == "Tutorial1" && missionInt == 12) //after walking into the mission trigger
        {
            missionInt += 1;
            PlayGlobalSoundOfIndex(5);
        }
        if (missionName == "Tutorial1" && missionInt == 13) //If player equips the fireball, move on.
        {
            var playerRefHolder = GlobalReferenceDatabase.globalreferencedatabase.Player.GetComponent<PlayerReferenceHolder>();
            if (playerRefHolder.leftMagicCaster.spellSelected == HandMagicCaster.spells.Fireball || playerRefHolder.rightMagicCaster.spellSelected == HandMagicCaster.spells.Fireball)
            {
                missionInt += 1;
            }
        }

        if (missionName == "Tutorial1" && missionInt == 14) //Spawns in fire dummies and plays sound
        {
            missionInt += 1;
            PlayGlobalSoundOfIndex(6);
            Tutorial1_missionElements_village[3].SetActive(true);
            counter1 = 0;

        }

        if (missionName == "Tutorial1" && missionInt == 15) //If all fire targets are destroyed, move on
        {
            if(counter1 == 4)
            {
                missionInt += 1;
            }
        }

        if (missionName == "Tutorial1" && missionInt == 16) //spawns ice dummies and plays sound
        {
            missionInt += 1;
            counter1 = 0;
            PlayGlobalSoundOfIndex(7);
            Tutorial1_missionElements_village[3].SetActive(false);
            Tutorial1_missionElements_village[4].SetActive(true);

        }
        if (missionName == "Tutorial1" && missionInt == 17) //When all dummies destroyed...
        {
            if(counter1 == 4)
            {
                missionInt += 1;
            }
        }
        if (missionName == "Tutorial1" && missionInt == 18) //Play the sound explaining and start timer
        {
            missionInt += 1;
            PlayGlobalSoundOfIndex(8);
            startTimer(36, true);
            

        }

        if (missionName == "Tutorial1" && missionInt == 20) //This fires up after the voice stops talking
        {
            missionInt += 1;
            Tutorial1_missionElements_village[1].GetComponent<Animator>().Play("PracticeProjectile");
            startTimer(27, true);

        }

        if (missionName == "Tutorial1" && missionInt == 22) //When the projectile defending is complete.
        {
            missionInt += 1;
            PlayGlobalSoundOfIndex(10);
            Tutorial1_missionElements_village[6].GetComponent<missionTrigger>().isActive = true;

        }
        if (missionName == "Tutorial1" && missionInt == 24) //When the trigger on the attic is entered. 
        {
            missionInt += 1;
            PlayGlobalSoundOfIndex(11);

        }

        if (missionName == "Tutorial1" && missionInt == 25) //Checks if the selection is made to the map...
        {
            if(Tutorial1_missionElements_village[7].GetComponent<TravelSceneSelectorMaster>().selectedSceneToLoad == "MistyForest")
            {
                missionInt += 1;
            }

        }

        if (missionName == "Tutorial1" && missionInt == 26) //spawns the magniglass
        {
            missionInt += 1;
            PlayGlobalSoundOfIndex(12);
            Tutorial1_missionElements_village[8].SetActive(true);
            Tutorial1_missionElements_village[9].SetActive(true);
            Tutorial1_missionElements_village[10].GetComponent<missionTrigger>().isActive = true;

        }


        if (missionName == "Tutorial1" && missionInt == 28) //when coming closer to the magni glass..
        {
            missionInt += 1;
            

        }




        #endregion

    }


    private void OnEverySceneChange()
    {
        Debug.Log("New Scene! Wow! Shiny!");
        //Find the tutorial elements container and fill it in. 
        var localStorage = GameObject.Find("LOCALREFERENCESCONTAINER");
        
        if(currentScene == "village")
        {
            Tutorial1_missionElements_village = localStorage.GetComponent<localReferenceStorage>().localMissionGameobjects;
            if(missionInt == 3)
            {
                missionInt = 4; //When loaded into the village on the missionInt3 state, push forward the mission.
                var player = GlobalReferenceDatabase.globalreferencedatabase.Player;
                player.GetComponent<Animator>().Play("BlackFadeOut");

            }
        }

        if (currentScene == "Ship")
        {
            Tutorial1_missionElements_Ship = localStorage.GetComponent<localReferenceStorage>().localMissionGameobjects;
        }

    }


    public void PlayGlobalSoundOfIndex(int index)
    {
        
        globalAudioSource.clip = myAudioPreloader.allAudioClips[index];
        globalAudioSource.Play();
    }

    public void Tutorial1_functions_activateDoorsKnock()
    {
        Tutorial1_missionElements_Ship[0].GetComponent<AudioSource>().Play();
        Tutorial1_missionElements_Ship[0].GetComponent<GrabActionInterfaceTrigger>().missionTrigger_canBeTriggered = true;
    }

    private void Tutorial1_functions_missionInt2()
    {
        //fade in the screen.
        var player = GlobalReferenceDatabase.globalreferencedatabase.Player;
        player.GetComponent<Animator>().Play("BlackFadeIn");
        //Run the intro sound.
        PlayGlobalSoundOfIndex(0);
        
    }

    public void Tutorial1_functions_goToVillage()
    {
        SceneManager.LoadScene("village");
    }


    public void textMarker(string text)
    {
        Debug.Log(text);
    }

    public void Tutorial1_functions_missionInt6()
    {
        Tutorial1_missionElements_village[0].GetComponent<Animator>().Play("Open"); //Open the chest and run effects
        Tutorial1_missionElements_village[1].SetActive(true);
        Tutorial1_missionElements_village[1].GetComponent<Animator>().Play("GuardianWakeUp");
    }

    public void missionTimer()
    {
        if(timerActive)
        {
            if(timerBuffer < timerTime)
            {
                timerBuffer += Time.deltaTime;
            }
            if(timerBuffer > timerTime)
            {
                
                if(timerProgressMission)
                {
                    missionInt += 1;
                }
                clearTimer();
            }
        }
    }

    public void startTimer(float time, bool progressTheMission)
    {
        timerBuffer = 0;
        timerTime = time;
        timerActive = true;
        timerProgressMission = progressTheMission;
    }

    public void clearTimer()
    {
        timerProgressMission = false;
        timerBuffer = 0;
        timerTime = 0;
        timerActive = false;
    }

}
