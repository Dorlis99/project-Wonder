using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    public bool isActive;
    [Tooltip("Chance is a number between 0 and 1000")]
    public int chanceToSpawn;
    public GameObject ingredientToSpawn;
    public Transform mySpawnTransform;
    [Tooltip("Array: Randomize in X Y Z axis.")]
    public bool[] randomizeRotation = new bool[3];




    private void Start()
    {
        if(isActive)
        {
            var randomNumber = Random.Range(0, 1000);
            if(chanceToSpawn >= randomNumber)
            {
                //successfull spawn!
                var ing = Instantiate(ingredientToSpawn, mySpawnTransform.position, mySpawnTransform.rotation);
                ing.GetComponent<Rigidbody>().isKinematic = true;

                Quaternion rotation = mySpawnTransform.transform.rotation;

                if(randomizeRotation[0])
                {
                    var random = Random.Range(0, 360);
                    rotation = Quaternion.Euler(random, rotation.y, rotation.z);
                }
                if (randomizeRotation[1])
                {
                    var random = Random.Range(0, 360);
                    rotation = Quaternion.Euler(rotation.x, random, rotation.z);
                }
                if (randomizeRotation[2])
                {
                    var random = Random.Range(0, 360);
                    rotation = Quaternion.Euler(rotation.x, rotation.y, random);
                }

                ing.transform.rotation = rotation;
            }

            
            Destroy(gameObject);
        }
    }
}
