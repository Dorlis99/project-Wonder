using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    public enum flaskContentTypes { none, liquid, solid}

    #region SaveDataVariables
    public string prefabName;

    #endregion


    private Animator myAnimator;
    public GameObject cork;
    public GameObject liquidInside;
    public GameObject insideObjectReference;
    // public Transform corkTransform;
    public Transform insideAnchorReference;


    public flaskContentTypes content;
    public string liquidFlaskContentName;
    public Color potionColor;
    public bool isEmpty;
    public bool isOpen;
    private objectHandler myOH;
    private bool toggleLocked;

    public AudioSource Audio_open;
    public AudioSource Audio_close;
    public AudioSource Audio_waterSplash;
    public ParticleSystem Particle_waterSplash;

    private bool isOnCooldown;
    private float cooldownBuffer;

    private bool isOnCooldown2;
    private float cooldownBuffer2;

    //public bool DEBUG_SHOWCOOLDOWN;

    private void Start()
    {
        updateBottleStatus();
        myAnimator = gameObject.GetComponent<Animator>();
        myOH = gameObject.GetComponent<objectHandler>();
    }


    private void Update()
    {
       // if(DEBUG_SHOWCOOLDOWN) //DEBUG ONLY!! DEBUG ONLY!! DEBUG ONLY!! DEBUG ONLY!!
      //  {
      //      Debug.Log("COOLDOWN: " + cooldownBuffer);
      //  }


        objectPickupCooldown();
        ingredientPickupCooldown();

        //This needs to be optimised later on:
        if (myOH.isGrabbed)
        {
            if(myOH.whichHandIsHolding == "L")
            {
                if(InputManager.inputManager.leftController_Trigger)
                {
                    if (toggleLocked == false)
                    {
                        toggleLocked = true;
                        toggleBottleOpenState();
                    }
                }

                

                
            }

            if (myOH.whichHandIsHolding == "R")
            {
                if (InputManager.inputManager.rightController_Trigger)
                {
                    if (toggleLocked == false)
                    {
                        toggleLocked = true;
                        toggleBottleOpenState();
                    }
                }
                


            }

            
        }

        if(isEmpty == false && isOpen)
        {
            if(isOnCooldown == false)
            {
                if (Vector3.Dot(gameObject.transform.up, Vector3.down) > 0.25)
                {
                    if(content == flaskContentTypes.solid)
                    {
                        isOnCooldown = true;
                        isOnCooldown2 = true;
                        //insideObjectReference.transform.localPosition += new Vector3(0, 0, 0.025f); //CHANGING THIS LINE FOR A DROP POSITION EQUAL TO PARTICLE SPLASH POINT
                        insideObjectReference.transform.localPosition = Particle_waterSplash.transform.localPosition;

                        isEmpty = true;
                        var ingredient = insideObjectReference.GetComponent<Ingredient>();
                        ingredient.becomeUnstored();
                        insideObjectReference.transform.parent = null;
                        insideObjectReference = null;
                        content = flaskContentTypes.none;
                    }

                    if(content == flaskContentTypes.liquid)
                    {
                        Particle_waterSplash.startColor = potionColor;
                        Particle_waterSplash.Play();
                        isOnCooldown = true;
                        isEmpty = true;
                        content = flaskContentTypes.none;
                        var droplet = Instantiate(GlobalReferenceDatabase.globalreferencedatabase.PotionWaterDropPrefab, Particle_waterSplash.transform.position, insideAnchorReference.transform.rotation);
                        var dropletScript = droplet.GetComponent<waterdropPotionHolder>();
                        dropletScript.potionColor = potionColor;
                        dropletScript.ingredientName = liquidFlaskContentName;
                        dropletScript.applyColorToChildren();
                        updateBottleLiquidStatus(Color.black);
                        potionColor = Color.black;
                        Audio_waterSplash.Play();
                        


                    }


                }
            }
            
           
        }

    }


    private void updateContent()
    {

    }

    private void dumpContent()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        
        //if(isEmpty || isOpen)
        //{
        //    //do stuff;
        //}

        //if(isOpen)
        //{
        //    if(other.gameObject.GetComponent<Cork>() != null) //if cork is detected
        //    {
        //        var corkscript = other.GetComponent<Cork>();
        //        other.transform.parent = gameObject.transform;
        //        corkscript.corkThatCork(gameObject);
        //        updateBottleStatus();
        //        isOpen = false;

        //    }
        //}
    }

    private void OnTriggerStay(Collider other)
    {
        if(isEmpty && isOpen && isOnCooldown2 == false)
        {
            if (other.TryGetComponent(out Ingredient ingredient))
            {
                if (ingredient.gameObject.GetComponent<objectHandler>().isGrabbed == false)
                {
                    
                    isEmpty = false;
                    ingredient.becomeStored();
                    content = flaskContentTypes.solid;
                    insideObjectReference = ingredient.gameObject;
                    insideObjectReference.transform.parent = insideAnchorReference;
                    insideObjectReference.transform.localPosition = Vector3.zero + ingredient.holdingCorrectionPosition;
                    insideObjectReference.transform.localRotation = Quaternion.Euler(Vector3.zero + ingredient.holdingCorrectionRotation);
                    isOnCooldown2 = true;

                }
            }

            if(other.TryGetComponent(out LiquidIngredientSource liquidIngredient))
            {
                if(liquidIngredient.isFinite)
                {
                    if(liquidIngredient.chargesLeft <= 0)
                    {
                        return;
                    }
                }

                isEmpty = false;
                content = flaskContentTypes.liquid;
                liquidFlaskContentName = liquidIngredient.ingredientName;
                if(liquidIngredient.isFinite)
                {
                    liquidIngredient.chargesLeft -= 1;
                }
                updateBottleLiquidStatus(liquidIngredient.liquidColor);
                potionColor = liquidIngredient.liquidColor;

            }

            if(other.TryGetComponent(out AlchemyCraftingStation AlchemyStation))
            {
                if(AlchemyStation.containsWater)
                {

                    isEmpty = false;
                    content = flaskContentTypes.liquid;
                    if(AlchemyStation.potentialPotion != "")
                    {
                        liquidFlaskContentName = AlchemyStation.potentialPotion;
                        AlchemyStation.togglePotionReadyPS(false);
                    }
                    else
                    {
                        liquidFlaskContentName = AlchemyStation.potionContent;

                    }
                    updateBottleLiquidStatus(AlchemyStation.potionColor);
                    potionColor = AlchemyStation.potionColor;
                    AlchemyStation.FillInBottle();
                    closeBottle();

                }
            }
        }
        
    }


    public void updateBottleStatus()
    {

        //if(cork == null)
        //{
        //    isOpen = true;
        //}
        //else
        //{
        //    isOpen = false;
        //}


    }

    public void openBottle() //Animator based
    {
        myAnimator.Play("openCork");
        isOpen = true;
        Audio_open.Play();
    }

    public void closeBottle() //Animator based
    {
        myAnimator.Play("closeCork");
        isOpen = false;
        Audio_close.Play();

    }

    public void toggleBottleOpenState()
    {
        if(isOpen)
        {
            closeBottle();
            return;
        }
        if (isOpen == false)
        {
            openBottle();
            return;
        }
    }

    public void lockAnimation()
    {
        toggleLocked = true;
    }

    public void unlockAnimation()
    {
        toggleLocked = false;
    }

    private void objectPickupCooldown()
    {
        if(isOnCooldown)
        {
            if(cooldownBuffer < 1)
            {
                cooldownBuffer += Time.deltaTime;
            }

            if(cooldownBuffer >= 1)
            {
                isOnCooldown = false;
                cooldownBuffer = 0;
            }
        }
    }

    private void ingredientPickupCooldown()
    {
        if (isOnCooldown2)
        {
            if (cooldownBuffer2 < 2)
            {
                cooldownBuffer2 += Time.deltaTime;
                Debug.Log("BOTTLE SCRIPT: Second cooldown is running...");
            }

            if (cooldownBuffer2 >= 2)
            {
                isOnCooldown2 = false;
                cooldownBuffer2 = 0;
                Debug.LogWarning("BOTTLE SCRIPT: Second cooldown is done!");
            }
        }
    }

    public void updateBottleLiquidStatus(Color liquidColor)
    {
        if(content == flaskContentTypes.liquid)
        {
            liquidInside.SetActive(true);
        }
        else
        {
            liquidInside.SetActive(false);
        }

        var mat = liquidInside.GetComponent<MeshRenderer>().material;
        mat.SetColor("_Color", liquidColor);
    }

    public void SAVE_overrideContent(flaskContentTypes OR_contentType, GameObject OR_solidContent, string OR_liquidName, Color OR_liquidColor)
    {
        if(OR_contentType == flaskContentTypes.none)
        {
            isEmpty = true;
            //Hopefully I don't have to program this..?
        }

        if(OR_contentType == flaskContentTypes.liquid)
        {
            content = flaskContentTypes.liquid;
            liquidFlaskContentName = OR_liquidName;
            potionColor = OR_liquidColor;
            isEmpty = false;
            updateBottleLiquidStatus(potionColor);

        }

        if(OR_contentType == flaskContentTypes.solid)
        {
            var ingredient = OR_solidContent.GetComponent<Ingredient>();
            isEmpty = false;
            ingredient.becomeStored();
            content = flaskContentTypes.solid;
            insideObjectReference = ingredient.gameObject;
            insideObjectReference.transform.parent = insideAnchorReference;
            insideObjectReference.transform.localPosition = Vector3.zero + ingredient.holdingCorrectionPosition;
            insideObjectReference.transform.localRotation = Quaternion.Euler(Vector3.zero + ingredient.holdingCorrectionRotation);
        }
    }
}
