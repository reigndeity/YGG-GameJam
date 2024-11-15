using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MopSpawner : MonoBehaviour
{
    public GameObject mopObstacle;
    public bool mopSpawn;
    public float mopMaxSpawnTime;
    public float mopMinSpawnTime;

    [Header("Spawners")]
    public GameObject[] upperRightSpawners;
    public GameObject[] lowerRightSpawners;
    public GameObject[] lowerLeftSpawners;
    public GameObject[] upperLeftSpawners;

    private void Update()
    {
        if (mopSpawn)
        {
            StartCoroutine(MopSpawn());
        }
    }

    IEnumerator MopSpawn()
    {
        mopSpawn = false;
        yield return new WaitForSeconds(Random.Range(mopMinSpawnTime, mopMinSpawnTime));

        RandomSpawner();

        mopSpawn = true;
    }

    public void RandomSpawner()
    {
        int randomSpawn = Random.Range(0, 4);
        int rotationAngle = 0;
        GameObject toSpawn;

        if (randomSpawn == 0 && upperRightSpawners[Random.Range(0, upperRightSpawners.Length)] != null) //Go Upper Right
        {
            toSpawn = upperRightSpawners[Random.Range(0, upperRightSpawners.Length)];
            rotationAngle = 45;
        }
        else if (randomSpawn == 1 && lowerRightSpawners[Random.Range(0, lowerRightSpawners.Length)] != null) //Go Lower Right
        {
            toSpawn = lowerRightSpawners[Random.Range(0, lowerRightSpawners.Length)];
            rotationAngle = 135;
        }
        else if (randomSpawn == 2 && lowerLeftSpawners[Random.Range(0, lowerLeftSpawners.Length)] != null) //Go Lower Left
        {
            toSpawn = lowerLeftSpawners[Random.Range(0, lowerLeftSpawners.Length)];
            rotationAngle = 225;
        }
        else if (randomSpawn == 3 && upperLeftSpawners[Random.Range(0, upperLeftSpawners.Length)] != null) //Go Upper Left
        {
            toSpawn = upperLeftSpawners[Random.Range(0, upperLeftSpawners.Length)];
            rotationAngle = 315;
        }
        else return;

        Quaternion rotation = Quaternion.Euler(0, rotationAngle, 0);
        GameObject mop = Instantiate(mopObstacle, toSpawn.transform.position, rotation);
    }
}
