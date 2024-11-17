using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezer : MonoBehaviour
{
    public Dictionary<GameObject, List<PlayerMovement>> trackedPlayer = new Dictionary<GameObject, List<PlayerMovement>>();

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player is Slowed");
            if (!trackedPlayer.ContainsKey(other.gameObject))
            {
                List<PlayerMovement> playerMovements = new List<PlayerMovement>(other.gameObject.GetComponents<PlayerMovement>());
                trackedPlayer.Add(other.gameObject, playerMovements);
                foreach (PlayerMovement playerMovement in playerMovements)
                {
                    playerMovement.isOnFreezer = true;
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        // You may not need to do anything here if OnTriggerEnter already handles the slowdown
    }

    void OnTriggerExit(Collider other)
    {
        if (trackedPlayer.ContainsKey(other.gameObject))
        {
            List<PlayerMovement> playerMovements = trackedPlayer[other.gameObject];

            foreach (PlayerMovement playerMovement in playerMovements)
            {
                playerMovement.isOnFreezer = false;
            }

            trackedPlayer.Remove(other.gameObject); // Remove from dictionary
        }
    }
}
