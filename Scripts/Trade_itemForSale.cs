using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trade_itemForSale : MonoBehaviour
{
    public bool thisItemCanBeSold;
    public int value;
    public bool overrideValue;
    public int valueOverriden;
    private PlayerTradeController theTradeController;
    private int loop1Index;

    private void Start()
    {
        theTradeController = GlobalReferenceDatabase.globalreferencedatabase.TheTradeController;
        reevaluateItemTradeStatus();

    }

    public void reevaluateItemTradeStatus()
    {
        if(gameObject.GetComponent<Bottle>() != null)
        {
            var bottle = gameObject.GetComponent<Bottle>();

            if(bottle.isEmpty)
            {
                thisItemCanBeSold = false;
                value = 0;
            }

            if(bottle.content == Bottle.flaskContentTypes.solid)
            {
                value = bottle.insideObjectReference.GetComponent<Ingredient>().monetaryValue;
                thisItemCanBeSold = true;
            }

            if(bottle.content == Bottle.flaskContentTypes.liquid)
            {
                thisItemCanBeSold = true;
                var potionName = bottle.liquidFlaskContentName;
                loop1Index = 0;
                if(theTradeController == null)
                {
                    theTradeController = GlobalReferenceDatabase.globalreferencedatabase.TheTradeController; //I needed to add this because it's fucking stupid for no reason
                }
                foreach(string name in theTradeController.customPricingName)
                {
                    if(name == potionName)
                    {
                        value = theTradeController.customPricingPrice[loop1Index];
                    }
                    else
                    {
                        loop1Index++;
                    }
                }
            }
        }

        if(overrideValue)
        {
            value = valueOverriden;
            thisItemCanBeSold = true;
        }
    }
}
