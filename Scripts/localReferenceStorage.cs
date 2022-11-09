using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class localReferenceStorage : MonoBehaviour
{

    public List<GameObject> localMissionGameobjects;



    public void passCounter1InfoToMissionControl(int count)
    {
        var missionControl = GameObject.FindObjectOfType<MissionControl>();
        missionControl.counter1 += 1;
    }
}
