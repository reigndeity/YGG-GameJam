using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Make sure to include this for the Image component

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public float deathTimer;
    public float deathCountdown;
    public bool canSpawn = true;

    [SerializeField] Image playerFill; // Image component for the fill effect
    [SerializeField] AudioManager _audioManager;

    void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        if (GameManager.instance.gameStart == true)
        {
            // Check if the player does not exist and we are allowed to spawn
            GameObject playerInstance = GameObject.Find(playerPrefab.name + "(Clone)");
            if (playerInstance == null && canSpawn)
            {
                StartCoroutine(SpawnPlayer());
            }

            // Update the countdown timer if the player is not present
            if (playerInstance == null)
            {
                deathCountdown -= Time.deltaTime;
                if (deathCountdown < 0)
                {
                    deathCountdown = 0;
                }

                // Update the fill amount based on the countdown
                playerFill.fillAmount = 1 - (deathCountdown / deathTimer);
            }
        }
    }

    IEnumerator SpawnPlayer()
    {
        canSpawn = false;
        deathCountdown = deathTimer;
        playerFill.fillAmount = 0; // Set the fill amount to 0 when starting the countdown

        // Wait for the countdown to finish
        yield return new WaitForSeconds(deathTimer);

        _audioManager.PlayRespawnSound();
        Instantiate(playerPrefab, transform.position, Quaternion.identity);
        playerFill.fillAmount = 1; // Set the fill amount to 1 when the player spawns

        canSpawn = true;
    }
}
