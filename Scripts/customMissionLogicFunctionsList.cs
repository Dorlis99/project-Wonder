using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customMissionLogicFunctionsList : MonoBehaviour
{
    public GameObject testProjectile;
    public void guardian_fireTestProjectile()
    {
        var projectile = Instantiate(testProjectile, gameObject.transform.position, gameObject.transform.rotation);
        projectile.transform.LookAt(GlobalReferenceDatabase.globalreferencedatabase.Player.transform);

    }


}
