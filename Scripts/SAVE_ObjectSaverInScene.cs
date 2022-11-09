using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SAVE_ObjectSaverInScene : MonoBehaviour
{
    private string savePath;
    private List<GameObject> activeSceneList;
    public string originalScene;

    private void Start()
    {
        //FAKE SAVE FEATURE:
        DontDestroyOnLoad(gameObject);


        var myScene = SceneManager.GetActiveScene();

        //First, make sure that the path for saving this location is correct and the correct list of items is pulled from the save file.
        if (myScene.name == "village")
        {
            originalScene = myScene.name;

            // activeSceneList = GlobalReferenceDatabase.globalreferencedatabase.SAVE_villageSsceneObjects;
            GlobalReferenceDatabase.globalreferencedatabase.Player.GetComponent<GameSaverAndManager>().FAKESAVE_villageObjects.Add(gameObject);
        }

        if (myScene.name == "MistyForest")
        {
            originalScene = myScene.name;
            // activeSceneList = GlobalReferenceDatabase.globalreferencedatabase.SAVE_forestSceneObjects;
            GlobalReferenceDatabase.globalreferencedatabase.Player.GetComponent<GameSaverAndManager>().FAKESAVE_forestObjects.Add(gameObject);

        }

    }

    public void addObjectForSaving()
    {
        activeSceneList.Add(gameObject);
    }

    private void OnDestroy()
    {
        subtractObjectFromSaving();
    }

    public void subtractObjectFromSaving()
    {
        if (originalScene == "village")
        {

            GlobalReferenceDatabase.globalreferencedatabase.Player.GetComponent<GameSaverAndManager>().FAKESAVE_villageObjects.Remove(gameObject);
        }

        if (originalScene == "MistyForest")
        {
            GlobalReferenceDatabase.globalreferencedatabase.Player.GetComponent<GameSaverAndManager>().FAKESAVE_forestObjects.Remove(gameObject);

        }
    }

}
