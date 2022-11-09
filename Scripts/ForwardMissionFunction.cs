using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardMissionFunction : MonoBehaviour
{

    public void pushMissionIndex()
    {
        var gm = GameObject.FindObjectOfType<MissionControl>();

        gm.missionInt += 1;
    }

    
}
