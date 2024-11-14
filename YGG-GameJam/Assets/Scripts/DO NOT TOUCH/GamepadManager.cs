using System.Collections.Generic;
using UnityEngine;

public class GamepadManager : MonoBehaviour
{
    [SerializeField] string[] controllers; // Array of connected controllers
    [SerializeField] Dictionary<int, string> playerControllers = new Dictionary<int, string>(); // Maps player IDs to controller names
    [SerializeField] int maxPlayers = 4; // Maximum number of players

    void Start()
    {
        AssignControllers();
    }

    void Update()
    {
        // Continuously check if a controller is connected or disconnected
        CheckForControllerChanges();
    }

    private void AssignControllers()
    {
        controllers = Input.GetJoystickNames(); // Get list of connected controllers

        int assignedPlayers = 0;
        for (int i = 0; i < controllers.Length; i++)
        {
            if (!string.IsNullOrEmpty(controllers[i]) && assignedPlayers < maxPlayers)
            {
                playerControllers[assignedPlayers + 1] = controllers[i];
                Debug.Log("Assigned " + controllers[i] + " to Player " + (assignedPlayers + 1));
                assignedPlayers++;
            }
        }

        // Log if not enough controllers for all players
        if (assignedPlayers < maxPlayers)
        {
            Debug.LogWarning("Not enough controllers connected for all players.");
        }
    }

    private void CheckForControllerChanges()
    {
        string[] currentControllers = Input.GetJoystickNames();

        // Reassign if the number of controllers has changed
        if (currentControllers.Length != controllers.Length)
        {
            Debug.Log("Controller count changed, reassigning controllers...");
            AssignControllers();
        }
    }

    public bool IsControllerAssignedToPlayer(int playerID)
    {
        return playerControllers.ContainsKey(playerID);
    }

    public string GetControllerNameForPlayer(int playerID)
    {
        return playerControllers.ContainsKey(playerID) ? playerControllers[playerID] : null;
    }
}
