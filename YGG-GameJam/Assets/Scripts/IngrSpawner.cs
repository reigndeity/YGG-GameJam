using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngrSpawner : MonoBehaviour
{
    public string ingrName;
    public GameObject ingrSpawner;
    public GameObject[] ingrPrefab;
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
        float xSpawn = Random.Range(-10, 10);
        float zSpawn = Random.Range(-10, 10);
        Vector3 spawnPoint = new Vector3(xSpawn, 10, zSpawn);
        Instantiate(ingrPrefab[Random.Range(0,ingrPrefab.Length)], spawnPoint, Quaternion.identity);
        yield return new WaitForSeconds(3f);
        canSpawn = true;
    }
}
