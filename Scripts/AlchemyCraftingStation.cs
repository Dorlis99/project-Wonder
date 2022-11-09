using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemyCraftingStation : MonoBehaviour
{
    public bool containsWater;
    public Color potionColor;
    public string potionContent;
    public MeshRenderer myMeshRenderer;
    private bool thisIsFirstIngredient;
    public string potentialPotion;

    public Color[] predefinedPotionColors;
    public ParticleSystem potionReadyPS;


    void Start()
    {
        myMeshRenderer.enabled = false;
        thisIsFirstIngredient = true;
    }

    void Update()
    {
        
    }

    public void fillWithWater()
    {
        myMeshRenderer.enabled = true;
        updateColor();
        containsWater = true;
        thisIsFirstIngredient = true;

    }

    public void updateColor()
    {
        gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", potionColor);
    }

    public void FillInBottle()
    {
        containsWater = false;
        myMeshRenderer.enabled = false;
        potionContent = "";
    }

    public void addIngredient(string ingredientName, Color color, bool isSubtractive)
    {
        if(ingredientName == "Water" && containsWater == false)
        {
            fillWithWater();
            potionColor = color;
            updateColor();
            return;
        }
        else
        {

            potionContent += ingredientName;

            if(isSubtractive)
            {
                potionColor = potionColor - color / 2;
            }
            if(isSubtractive == false)
            {
                potionColor = potionColor + color / 2;
            }
             
            

            updateColor();
            PotionProofingScript();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //if (TryGetComponent(out Ingredient ing))
       // {
       //     if (containsWater)
       //     {
      //          addIngredient(ing.ingredientNameInPotion, ing.ingredientColorInPotion);
       //         Destroy(ing.gameObject);
       //     }
       // }

        var ing = other.GetComponent<Ingredient>();
        
        addIngredient(ing.ingredientNameInPotion, ing.ingredientColorInPotion, ing.colorIsSubtractive);
        ing.ingredientNameInPotion = "";
        Destroy(ing.gameObject);

    }


    private void PotionProofingScript()
    {
        if(potionContent == "PinkcreeperPinkcreeperBluemoon")
        {
            potentialPotion = "Vitality Potion";
            potionColor = predefinedPotionColors[0];
            updateColor();
            togglePotionReadyPS(true);
            return;
        }

        if (potionContent == "ShinyblessingDoestoolWine")
        {
            potentialPotion = "Flu Remedy";
            potionColor = predefinedPotionColors[1];
            updateColor();
            togglePotionReadyPS(true);
            return;
        }

        if (potionContent == "ShinyblessingPinkcreeperBluemoonDoestool")
        {
            potentialPotion = "Mushroom Tea";
            potionColor = predefinedPotionColors[2];
            updateColor();
            togglePotionReadyPS(true);
            return;
        }

       

        if (potionContent == "BluemoonBluemoonBluemoonPinkcreeperWine")
        {
            potentialPotion = "Glowing Decot";
            potionColor = predefinedPotionColors[3];
            updateColor();
            togglePotionReadyPS(true);
            return;
        }


        else
        {
            potentialPotion = "";
        }
    }

    public void togglePotionReadyPS(bool isItReady)
    {
        if(isItReady)
        {
            potionReadyPS.Play();
            potionReadyPS.startColor = potionColor;
        }
        if(isItReady == false)
        {
            potionReadyPS.Stop();
        }
    }
}
