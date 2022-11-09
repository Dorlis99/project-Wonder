using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class GameSaverAndManager : MonoBehaviour
{
    public GameObject gameLoadingScreen;
    public Text LS_title;
    public Text LS_content;

    public PlayerBag bag1;

    //Scene selection
    private string environmentSavePath;

    public List<GameObject> FAKESAVE_villageObjects;
    public List<GameObject> FAKESAVE_forestObjects;


    

    public void initialiseGameSave()
    {
        gameLoadingScreen.SetActive(true);
        determineSceneSaveParameters();
        FAKEsaveEnvironment();
        DataSaveSystem.SaveAll(bag1, LS_content);
        hideHUD();
    }


    public void initialiseGameLoad()
    {


        LS_content.text = "Loading player data...";
        gameLoadingScreen.SetActive(true);

        bag1 = GameObject.Find("PlayerBag(Clone)").GetComponent<PlayerBag>();
        bag1.openBag();
        SAVE_PlayerStatus s_PlayerStatus = DataSaveSystem.LoadPlayerStatus();
        if(s_PlayerStatus == null)
        {
            return;
        }

        foreach(GameObject bagSocket in bag1.potionSlots)
        {
            Destroy(bagSocket.GetComponent<potionStandScript>().referenceToBottle);
            bagSocket.GetComponent<potionStandScript>().unStore();
        }

        #region Load Player Data
        for (int i = 0; i < 8; i++) 
        {
            if(s_PlayerStatus.bag1SlotsBottleNames[i] != null)
            {
                var spawnedBottle = Instantiate(Resources.Load(s_PlayerStatus.bag1SlotsBottleNames[i]) as GameObject, Vector3.zero, Quaternion.Euler(0, 0, 0));

                if(s_PlayerStatus.bag1FlasksContentTypes[i] == Bottle.flaskContentTypes.liquid)
                {
                    var colorSource = s_PlayerStatus.bag1LiquidColors[i].ColorRGBA;
                    var potionColor = new Color(colorSource[0], colorSource[1], colorSource[2], colorSource[3]);
                    spawnedBottle.GetComponent<Bottle>().SAVE_overrideContent(Bottle.flaskContentTypes.liquid, null, s_PlayerStatus.bag1LiquidContentNames[i], potionColor);

                }

                if (s_PlayerStatus.bag1FlasksContentTypes[i] == Bottle.flaskContentTypes.solid)
                {
                    var ingredient = Instantiate(Resources.Load(s_PlayerStatus.bag1SolidContentPrefabsNames[i]) as GameObject, Vector3.zero, Quaternion.Euler(0, 0, 0));
                    spawnedBottle.GetComponent<Bottle>().SAVE_overrideContent(Bottle.flaskContentTypes.solid, ingredient, "", Color.black);
                }


                    bag1.potionSlots[i].GetComponent<potionStandScript>().OverrideStoredItem(spawnedBottle);

            }
        }


        #endregion

        #region Load environment data

        FAKEloadEnvironment();

        #endregion

        hideHUD();
        bag1.closeBag();

    }

    public void hideHUD()
    {
        gameLoadingScreen.SetActive(false);
    }

    private void Start()
    {
        //createFileSystem();
    }

    public void createFileSystem()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/SavedData"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/SavedData");
        }
        if (!Directory.Exists(Application.persistentDataPath + "/SavedData/playerStatus.sav"))
        {
            File.Create(Application.persistentDataPath + "/SavedData/playerStatus.sav");
        }
        if (!Directory.Exists(Application.persistentDataPath + "/SavedData/Environment_village.sav"))
        {
            File.Create(Application.persistentDataPath + "/SavedData/Environment_village.sav");//not used yet, moving onto more important things.
            File.Create(Application.persistentDataPath + "/SavedData/Environment_MistyForest.sav");//not used yet, moving onto more important things.
        }




    }

    public void determineSceneSaveParameters() //not used yet, moving onto more important things.
    {

        var myScene = SceneManager.GetActiveScene();

        //First, make sure that the path for saving this location is correct and the correct list of items is pulled from the save file.
        if (myScene.name == "village")
        {
            environmentSavePath = Application.persistentDataPath + "/SavedData/Environment_village.sav";
            //activeSceneList = GlobalReferenceDatabase.globalreferencedatabase.SAVE_villageSsceneObjects;
        }

        if (myScene.name == "MistyForest")
        {
            environmentSavePath = Application.persistentDataPath + "/SavedData/Environment_MistyForest.sav";
            //activeSceneList = GlobalReferenceDatabase.globalreferencedatabase.SAVE_forestSceneObjects;
        }
    }

    public void FAKEsaveEnvironment()
    {
        var myScene = SceneManager.GetActiveScene();

        if (myScene.name == "village")
        {
            //it's saved automatically now.
        }

        if (myScene.name == "MistyForest")
        {
            //it's saved automatically now. 
        }

    }

    public void FAKEloadEnvironment()
    {
        var myScene = SceneManager.GetActiveScene();

        if (myScene.name == "village")
        {
            foreach(GameObject obj in FAKESAVE_villageObjects)
            {
                obj.SetActive(true);
            }
            foreach(GameObject obj in FAKESAVE_forestObjects)
            {
                obj.SetActive(false);
            }
        }

        if (myScene.name == "MistyForest")
        {
            foreach (GameObject obj in FAKESAVE_villageObjects)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in FAKESAVE_forestObjects)
            {
                obj.SetActive(true);
            }
        }

        
    }
}
