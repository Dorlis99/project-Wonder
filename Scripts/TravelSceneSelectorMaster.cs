using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelSceneSelectorMaster : MonoBehaviour
{

    public string selectedSceneToLoad;
    public GrabActionInterfaceTrigger mainGate;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void contactMainGate()
    {
        mainGate.levelLoad_levelname = selectedSceneToLoad;
    }

    public void clearMaingate()
    {
        mainGate.levelLoad_levelname = "";
    }
}
