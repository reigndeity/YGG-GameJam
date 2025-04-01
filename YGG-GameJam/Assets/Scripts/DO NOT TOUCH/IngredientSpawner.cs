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

    private List<int> burgerCycle = new List<int>(); // For Burger Recipe
    private List<int> hotdogCycle = new List<int>(); // For Hotdog Recipe
    private List<int> sandwichCycle = new List<int>(); // For Sandwich Recipe
    private int currentBurgerIndex = 0;
    private int currentHotdogIndex = 0;
    private int currentSandwichIndex = 0;

    // To keep track of the last used spawn points (avoid using the same one three times)
    private List<int> lastUsedSpawnPoints = new List<int>();

    void Start()
    {
        InitializeCycles(); // Set up the shuffled lists for each recipe type
    }

    void Update()
    {
        if (canIngredientSpawn && GameManager.instance.gameStart)
        {
            canIngredientSpawn = false;
            SpawnIngredient();
        }
    }

    void InitializeCycles()
    {
        // Burger Cycle
        burgerCycle = GetShuffledList(0, 3);
        // Hotdog Cycle
        hotdogCycle = GetShuffledList(3, 6);
        // Sandwich Cycle
        sandwichCycle = GetShuffledList(6, 9);
    }

    List<int> GetShuffledList(int start, int count)
    {
        List<int> list = new List<int>();
        for (int i = start; i < count; i++)
        {
            list.Add(i);
        }
        // Shuffle the list
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }

    public void SpawnIngredient()
    {
        int randomSpawnPoint = GetRandomSpawnPoint();
        int ingredientIndex = 0;

        switch (GameManager.instance.recipeChosen)
        {
            case 0: 
                ingredientIndex = burgerCycle[currentBurgerIndex++];
                if (currentBurgerIndex >= burgerCycle.Count)
                {
                    currentBurgerIndex = 0;
                    burgerCycle = GetShuffledList(0, 3);
                }
                break;

            case 1:
                ingredientIndex = hotdogCycle[currentHotdogIndex++];
                if (currentHotdogIndex >= hotdogCycle.Count)
                {
                    currentHotdogIndex = 0;
                    hotdogCycle = GetShuffledList(3, 3);
                }
                break;

            case 2:
                ingredientIndex = sandwichCycle[currentSandwichIndex++];
                if (currentSandwichIndex >= sandwichCycle.Count)
                {
                    currentSandwichIndex = 0;
                    sandwichCycle = GetShuffledList(6, 9);
                }
                break;
        }

        Instantiate(ingredientObjs[ingredientIndex], ingredientSpawnPoints[randomSpawnPoint].position, Quaternion.identity);
        
        // Delay before next spawn
        currentSpawnTimeInterval = Random.Range(0, spawnTimeInterval);
        Invoke(nameof(SpawnTimeInterval), currentSpawnTimeInterval);
    }

    int GetRandomSpawnPoint()
    {
        int randomSpawnPoint;
        do
        {
            randomSpawnPoint = Random.Range(0, ingredientSpawnPoints.Length);
        }
        while (lastUsedSpawnPoints.Contains(randomSpawnPoint)); // Avoid the spawn point being used 3 times in a row

        // Add the new spawn point to the last used list
        if (lastUsedSpawnPoints.Count >= 2)
        {
            lastUsedSpawnPoints.RemoveAt(0); // Remove the oldest entry if there are 3 spawn points in the list
        }
        lastUsedSpawnPoints.Add(randomSpawnPoint);

        return randomSpawnPoint;
    }

    void SpawnTimeInterval()
    {
        canIngredientSpawn = true;
    }
}
