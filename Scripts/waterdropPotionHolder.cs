using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterdropPotionHolder : MonoBehaviour
{

    public string ingredientName;
    public Color potionColor;


    void Start()
    {
        applyColorToChildren();
        Destroy(gameObject, 2);
    }

    void Update()
    {
        
    }

    public void applyColorToChildren()
    {
        gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", potionColor);
        int children = transform.childCount;
        for (int i = 0; i < children; ++i)
        {
            var gm = gameObject.transform.GetChild(i);
            gm.GetComponent<MeshRenderer>().material.SetColor("_Color" ,potionColor);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.TryGetComponent(out AlchemyCraftingStation alchemyStation))
        {
            Debug.LogError("waterdrop stored in station!");
            alchemyStation.addIngredient(ingredientName, potionColor, false);
            Destroy(gameObject);
        }


    }
}
