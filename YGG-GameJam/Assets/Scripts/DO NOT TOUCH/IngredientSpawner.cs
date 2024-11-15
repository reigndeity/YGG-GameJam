using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] ingredientObjs;
    [SerializeField] Transform[] ingredientSpawnPoints;
    [SerializeField] bool canIngredientSpawn;
    [SerializeField] int spawnTimeInterval;
    [SerializeField] int currentSpawnTimeInterval;


    void Update()
    {
        if (canIngredientSpawn == true)
        {
            SpawnIngredient();
            Invoke("SpawnTimeInterval", currentSpawnTimeInterval);
        }
    }

   public void SpawnIngredient()
   {
        int randomCurrentIngredients;
        int randomSpawnPoint = Random.Range(0, ingredientSpawnPoints.Length);
        switch (GameManager.instance.recipeChosen) //0:Burger | 1:Sandwich | 2:Hotdogs
        {
            // Burger Recipe Type
            case 0:
                randomCurrentIngredients = Random.Range(0, 3);
                Instantiate(ingredientObjs[randomCurrentIngredients],ingredientSpawnPoints[randomSpawnPoint]);
                break;
            // Hotdog Recipe Type
            case 1:
                randomCurrentIngredients = Random.Range(3, 6); 
                Instantiate(ingredientObjs[randomCurrentIngredients],ingredientSpawnPoints[randomSpawnPoint]);     
                break;
            // Sandwich Recipe Type
            case 2:
                randomCurrentIngredients = Random.Range(6, 9);
                Instantiate(ingredientObjs[randomCurrentIngredients],ingredientSpawnPoints[randomSpawnPoint]);
                break;
        }
        currentSpawnTimeInterval = Random.Range(0,spawnTimeInterval);
        canIngredientSpawn = false;
   }
   void SpawnTimeInterval()
   {
        canIngredientSpawn = true;
   }

}
