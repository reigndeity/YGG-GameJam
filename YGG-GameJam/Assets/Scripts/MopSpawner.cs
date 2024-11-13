using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MopSpawner : MonoBehaviour
{
    /*private int xMinSpawn;
    private int xMaxSpawn;
    private int zMinSpawn;
    private int zMaxSpawn;*/

    public GameObject mopObstacle;
    public GameObject[] spawners;
    // 45 = Going Upper Right, 1-5
    // 135 = Going Lower Right 6-10
    // 225 = Going Lower Left 11-15
    // 315 = Going Upper Left 16-20
    public bool mopSpawn;
    public float mopSpawnTime;

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
        yield return new WaitForSeconds(mopSpawnTime);

        int randomSpawner = Random.Range(1, spawners.Length + 1);
        int rotationAngle = 0;

        if (randomSpawner >= 1 && randomSpawner <= 5) //From Upper Right, should go Lower Left
        {
            rotationAngle = 225;
        } 
        else if (randomSpawner >= 6 && randomSpawner <= 10) // From Lower Right, should go Upper Left
        {
            rotationAngle = 315;
        }
        else if (randomSpawner >= 11 && randomSpawner <= 15) // From Lower Left, should go Upper Right
        {
            rotationAngle = 45;
        }
        else if (randomSpawner >= 16 && randomSpawner <= 20) // From Upper Left, should go Lower Right
        {
            rotationAngle = 135;
        }

        Quaternion rotation = Quaternion.Euler(0, rotationAngle, 0);
        GameObject mop = Instantiate(mopObstacle, spawners[randomSpawner - 1].transform.position, rotation);

        mopSpawn = true;
        Debug.Log(rotation);
        Debug.Log("Spawned at " + randomSpawner);
    }
}
