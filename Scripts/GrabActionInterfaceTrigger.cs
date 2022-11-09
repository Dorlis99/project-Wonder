using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GrabActionInterfaceTrigger : MonoBehaviour
{
    public enum GAI_actions { None, Portal, LoadLevel, missionTrigger };
    public string publicTitle;
    public string mission_revealedMissionTitle;
    public string levelLoad_levelname;
    public GAI_actions thisTriggerAction;
    public bool triggerFunction;
    public AudioSource optionalSoundEffect;
    private bool missionTriggerIsTriggered;
    private MissionControl theMissionControl;
    public bool missionTrigger_canBeTriggered;

    public Transform portalTeleportPosition;

    void Start()
    {
        theMissionControl = GameObject.FindObjectOfType<MissionControl>(); //There should be just one!!!
    }

    void Update()
    {
        GAI_functionSelector();
    }


    //GAI FUNCTIONS:

    public void GAI_functionSelector()
    {
        switch (thisTriggerAction)
        {
            case GAI_actions.None:
                return;

            case GAI_actions.Portal:
                GAI_PortalFunction();
                break;

            case GAI_actions.LoadLevel:
                GAI_LoadNextLevel();
                break;

            case GAI_actions.missionTrigger:
                GAI_missionTrigger();
                break;
        }

        
    }

    public void GAI_PortalFunction()
    {
        if (triggerFunction == true)
        {
            Debug.LogWarning("GAI: Portal function activated!");
            triggerFunction = false;
            GlobalReferenceDatabase.globalreferencedatabase.Player.transform.position = portalTeleportPosition.position;
            GlobalReferenceDatabase.globalreferencedatabase.Player.transform.rotation = portalTeleportPosition.rotation;
            if (optionalSoundEffect != null)
            {
                optionalSoundEffect.Play();
            }
        }
    }

    public void GAI_LoadNextLevel()
    {
        if (levelLoad_levelname == "")
        {
            publicTitle = "Select travel location at home first!";
            triggerFunction = false;
            return;
        }

        else
        {
            publicTitle = "Travel to: " + levelLoad_levelname;

            if (triggerFunction)
            {
                GlobalReferenceDatabase.globalreferencedatabase.Player.GetComponent<GameSaverAndManager>().initialiseGameSave(); //Start saving player.
                GlobalReferenceDatabase.globalreferencedatabase.Player.GetComponent<GameSaverAndManager>().LS_content.text = "Loading scene...";

                triggerFunction = false;
                SceneManager.LoadScene(levelLoad_levelname);
            }
        }
    }

    //This mission trigger works in a way that it will just progress the mission controller by 1 int. It needs to be locked untill it is needed. 
    public void GAI_missionTrigger()
    {
        if(missionTrigger_canBeTriggered)
        {
            publicTitle = mission_revealedMissionTitle;
        }
        if(missionTrigger_canBeTriggered == false)
        {
            publicTitle = "";
        }


        if(triggerFunction)
        {
            triggerFunction = false;
            if (missionTrigger_canBeTriggered)
            {
                if (missionTriggerIsTriggered == false)
                {
                    missionTriggerIsTriggered = true;
                    theMissionControl.missionInt += 1;
                    publicTitle = "";

                }
            }
        }

       
        
    }

    public void GAI_MissionTrigger_clearTrigger()
    {
        missionTriggerIsTriggered = false;
    }
}
