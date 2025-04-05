using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] ingredientObjs;
    [SerializeField] Transform spawnCenter; // Center of the spawn area
    [SerializeField] float spawnSize = 5f; // Size of the square area
    [SerializeField] bool canIngredientSpawn;
    [SerializeField] int spawnTimeInterval;
    [SerializeField] int currentSpawnTimeInterval;

    [SerializeField] float rotationAngle = 0f; // Rotation angle of the square (in degrees)

    private List<int> burgerCycle = new List<int>();
    private List<int> hotdogCycle = new List<int>();
    private List<int> sandwichCycle = new List<int>();
    private int currentBurgerIndex = 0;
    private int currentHotdogIndex = 0;
    private int currentSandwichIndex = 0;

    void Start()
    {
        InitializeCycles();
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
        burgerCycle = GetShuffledList(0, 3);
        hotdogCycle = GetShuffledList(3, 6);
        sandwichCycle = GetShuffledList(6, 9);
    }

    List<int> GetShuffledList(int start, int count)
    {
        List<int> list = new List<int>();
        for (int i = start; i < count; i++)
        {
            list.Add(i);
        }
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
        Vector3 spawnPosition = GetRandomPositionInRotatedSquare(); // Get position in rotated square
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
                    hotdogCycle = GetShuffledList(3, 6);
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

        Instantiate(ingredientObjs[ingredientIndex], spawnPosition, ingredientObjs[ingredientIndex].transform.rotation);

        currentSpawnTimeInterval = Random.Range(0, spawnTimeInterval);
        Invoke(nameof(SpawnTimeInterval), currentSpawnTimeInterval);
    }

    Vector3 GetRandomPositionInRotatedSquare()
    {
        // Random X and Z positions within the square range
        float randomX = Random.Range(-spawnSize / 2f, spawnSize / 2f);
        float randomZ = Random.Range(-spawnSize / 2f, spawnSize / 2f);

        // Calculate the random position in world space with rotation applied
        Vector3 randomPosition = new Vector3(randomX, 0f, randomZ);

        // Apply rotation to the random position around the spawn center
        randomPosition = Quaternion.Euler(0f, rotationAngle, 0f) * randomPosition;

        // Use the spawnCenter as the base position and add the rotated random position
        return spawnCenter.position + randomPosition;
    }

    void SpawnTimeInterval()
    {
        canIngredientSpawn = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green; // Set the gizmo color

        // Draw the square with rotation
        Gizmos.matrix = Matrix4x4.TRS(spawnCenter.position, Quaternion.Euler(0f, rotationAngle, 0f), Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(spawnSize, 0.1f, spawnSize)); // Draw a square at the center
    }
}
