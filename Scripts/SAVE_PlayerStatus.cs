using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SAVE_PlayerStatus
{

    //IMPORTANT EDITS:
    //Edit 1: 27/04 Changed hardcoded data from 4 to 8, as added new bottle slots into the bag.
    //DATA variables
    public string[] bag1SlotsBottleNames = new string[8];
    public Bottle.flaskContentTypes[] bag1FlasksContentTypes = new Bottle.flaskContentTypes[8];
    public string[] bag1SolidContentPrefabsNames = new string[8];
    public string[] bag1LiquidContentNames = new string[8];
    public SAVE_savedColor[] bag1LiquidColors = new SAVE_savedColor[8];
    



    public SAVE_PlayerStatus (PlayerBag bag)
    {
        Debug.LogWarning("Player status is saving...");

        for(int i = 0; i < 8; i++) //HARDCODED FOR 4 (8) LOOPS. KEEP THAT IN MIND!
        {
            if(bag.potionSlots[i].GetComponent<potionStandScript>().referenceToBottle != null) //IF there is a bottle in a corresponding [i] slot of the bag:
            {
                bag1SlotsBottleNames[i] = bag.potionSlots[i].GetComponent<potionStandScript>().referenceToBottle.GetComponent<Bottle>().prefabName; //[i] stored bottle name is set to potion slot's potion stand's script containing bottle from which data is downloaded.

            }
        }

        for(int i = 0; i<8; i++) //STORE BOTTLE CONTENT TYPE
        {
            if(bag1SlotsBottleNames[i] != null)
            {
                var Currentbottle = bag.potionSlots[i].GetComponent<potionStandScript>().referenceToBottle.GetComponent<Bottle>();
                bag1FlasksContentTypes[i] = Currentbottle.content;

                
            }
        }
        for (int i = 0; i < 8; i++)
        {
            if(bag1FlasksContentTypes[i] != Bottle.flaskContentTypes.none) //IF there is nothing in the bottle, do nothing.
            {

                if(bag1FlasksContentTypes[i] == Bottle.flaskContentTypes.solid) //IF the content is solid, store prefab name of the ingredient.
                {
                    bag1SolidContentPrefabsNames[i] = bag.potionSlots[i].GetComponent<potionStandScript>().referenceToBottle.GetComponent<Bottle>().insideObjectReference.GetComponent<Ingredient>().prefabName; //GET prefab name from the bottle content object.
                }

                if(bag1FlasksContentTypes[i] == Bottle.flaskContentTypes.liquid) //IF the content is liquid, store color and name.
                {
                    var colorFromBottle = bag.potionSlots[i].GetComponent<potionStandScript>().referenceToBottle.GetComponent<Bottle>().potionColor;
                    bag1LiquidColors[i] = new SAVE_savedColor(colorFromBottle.r, colorFromBottle.g, colorFromBottle.b, colorFromBottle.a);

                    bag1LiquidContentNames[i] = bag.potionSlots[i].GetComponent<potionStandScript>().referenceToBottle.GetComponent<Bottle>().liquidFlaskContentName;
                }

                
            }
        }

    }
}