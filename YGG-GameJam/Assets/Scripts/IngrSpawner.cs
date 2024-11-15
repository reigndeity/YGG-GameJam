using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngrSpawner : MonoBehaviour
{
    //public string ingrName;
    //public GameObject ingrSpawner;
    public GameObject[] ingrPrefab;
    public GameObject[] ingrSpawnPoints;
    public int ingrCount;
    public int totalIngrCount;
    public bool canSpawn;

    private void Update()
    {
        if (canSpawn && ingrCount != totalIngrCount)
        {
            StartCoroutine(SpawnIngr());
        }
    }

    IEnumerator SpawnIngr()
    {
        canSpawn = false;
        ingrCount++;
        GameObject whereToSpawn = ingrSpawnPoints[Random.Range(0, ingrSpawnPoints.Length)];
        Instantiate(ingrPrefab[Random.Range(0,ingrPrefab.Length)], whereToSpawn.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(3f);
        canSpawn = true;
    }
}
