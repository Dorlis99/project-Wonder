using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalReferenceDatabase : MonoBehaviour
{
    public static GlobalReferenceDatabase globalreferencedatabase;


    public GameObject Player;
    public GameObject PlayerBagAnchor;
    public GameObject PotionWaterDropPrefab;
    public Transform PlayerEnemyProjectileTarget;
    public GameObject globalMainPlayerCamera;

    public List<GameObject> SAVE_villageSsceneObjects;
    public List<GameObject> SAVE_forestSceneObjects;
    public bool wasPlayerSpawned;
    public PlayerTradeController TheTradeController;
    public GameObject coinPrefab;
    public GameObject toolsMenuPrefab;
    public MissionControl theMissionControl;


    public NPC_navpoint[] villageNavigationPointsNPC;


 

    private void Awake()
    {
        globalreferencedatabase = gameObject.GetComponent<GlobalReferenceDatabase>();

        

    }
    private void Start()
    {
       
    }

    private void LateUpdate()
    {
        if(SceneManager.GetActiveScene().name == "village")
        {
            if(TheTradeController == null)
            {
                var tradeGO = GameObject.Find("PlayerTradeController");
                TheTradeController = tradeGO.GetComponent<PlayerTradeController>();

                //Sets the NPC nav points:
                var allNavPoints = GameObject.FindObjectsOfType<NPC_navpoint>();
                villageNavigationPointsNPC = allNavPoints;
            }
        }
    }

}
