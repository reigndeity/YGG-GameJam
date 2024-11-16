using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public float deathTimer;
    public float deathCountdown;
    public bool canSpawn;

    private void Update()
    {
        GameObject playerInstance = GameObject.Find(playerPrefab.name + "(Clone)");
        if (playerInstance == null && canSpawn)
        {           
            StartCoroutine(SpawnPlayer());
        }

        deathCountdown -= Time.deltaTime;
        if (deathCountdown <= 0) deathCountdown = 0;
    }

    IEnumerator SpawnPlayer()
    {
        canSpawn = false;
        deathCountdown = deathTimer;
        yield return new WaitForSeconds(deathTimer);
        Instantiate(playerPrefab, this.gameObject.transform.position, Quaternion.identity);

        canSpawn = true;

    }
}
