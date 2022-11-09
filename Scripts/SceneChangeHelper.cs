using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeHelper : MonoBehaviour
{
    public Transform OnSceneLoadTeleportLocation;
    public float timeout;
    private float timebuffer;
    public GameObject PlayerXRrigPrefab;
    public GameObject bag1Prefab;
    public GameObject playerCoinSackPrefab;


    //THIS FUNCTION was moved from START to AWAKE
    private void Awake()
    {
        if (GlobalReferenceDatabase.globalreferencedatabase.wasPlayerSpawned == false)
        {
            var player = Instantiate(PlayerXRrigPrefab);
            GlobalReferenceDatabase.globalreferencedatabase.wasPlayerSpawned = true;
            var bag1 = Instantiate(bag1Prefab);
            GlobalReferenceDatabase.globalreferencedatabase.Player = player;
            var coinSack = Instantiate(playerCoinSackPrefab);
            coinSack.GetComponent<coinStorage>().playerInventoryAnchor = player.GetComponent<CharacterMovement>().coinSackAnchor;
        }
    }

    

    private void Update()
    {
        if(timebuffer < timeout)
        {
            timebuffer += 1 * Time.deltaTime;
        }
        if(timebuffer>=timeout)
        {
            timebuffer = 0;
            timeout = 900;
            RunLoadRutine();
            
        }
    }

    public void RunLoadRutine()
    {
        
        GlobalReferenceDatabase.globalreferencedatabase.Player.GetComponent<GameSaverAndManager>().initialiseGameLoad();
        GlobalReferenceDatabase.globalreferencedatabase.Player.GetComponent<GameSaverAndManager>().hideHUD();
        GlobalReferenceDatabase.globalreferencedatabase.Player.transform.position = OnSceneLoadTeleportLocation.position;
        GlobalReferenceDatabase.globalreferencedatabase.Player.transform.rotation = OnSceneLoadTeleportLocation.rotation;
        ExitSceneChanger();
    }

    public void ExitSceneChanger()
    {
        Destroy(gameObject);
    }
}
