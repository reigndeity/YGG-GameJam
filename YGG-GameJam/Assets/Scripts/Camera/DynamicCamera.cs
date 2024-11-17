using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
    public List<Transform> players = new List<Transform>(); //public Transform[] players; // Array to hold the players' transforms
    public Vector3 offset; // Offset to position the camera behind the players
    public float minZoom = 5f; // Minimum orthographic size (closer zoom)
    public float maxZoom = 20f; // Maximum orthographic size (further zoom)
    public float zoomLimiter = 50f; // Factor to control zoom sensitivity

    private Vector3 velocity; // Velocity for smooth damp
    public float smoothTime = 0.5f; // Smoothing factor for movement

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (players.Count != 0)  //from players.Length
        {
            // Update the list of players (you can opt to add more logic to detect new player spawn)
            UpdatePlayerList(); // Changes

            MoveCamera();
            ZoomCamera();
        }
        else
        {
            UpdatePlayerList();
            CenterCamera();
        }
    }

    void MoveCamera()
    {
        // Calculate the center point of all players
        Vector3 centerPoint = GetCenterPoint();

        // Calculate the new position of the camera based on the center point and offset
        Vector3 newPosition = centerPoint + offset;

        // Freeze the Y component by using the current Y position of the camera
        newPosition.y = transform.position.y;

        // Smoothly move the camera to the new position
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    void ZoomCamera()
    {
        // Calculate the greatest distance between players
        float greatestDistance = GetGreatestDistance();

        // Adjust the orthographic size based on the distance between players
        float newZoom = Mathf.Lerp(maxZoom, minZoom, greatestDistance / zoomLimiter);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }

    Vector3 GetCenterPoint()
    {
        if (players.Count == 1)// from players.Length
        {
            return players[0].position;
        }

        // Calculate the bounding box that encapsulates all players
        var bounds = new Bounds(players[0].position, Vector3.zero);
        for (int i = 1; i < players.Count; i++)// from players.Length
        {
            bounds.Encapsulate(players[i].position);
        }

        return bounds.center;
    }

    float GetGreatestDistance()
    {
        // Calculate the bounding box that encapsulates all players
        var bounds = new Bounds(players[0].position, Vector3.zero);
        for (int i = 1; i < players.Count; i++)// from players.Length
        {
            bounds.Encapsulate(players[i].position);
        }

        // Return the largest dimension of the bounding box
        return bounds.size.x > bounds.size.z ? bounds.size.x : bounds.size.z;
    }

    //===============Changes==========================
    void UpdatePlayerList()
    {
        // Find and add any new players that aren't in the list yet
        PlayerMovement[] playerMovements = FindObjectsOfType<PlayerMovement>();
        foreach (var playerMovement in playerMovements)
        {
            if (!players.Contains(playerMovement.transform))
            {
                players.Add(playerMovement.transform);
            }
        }

        // Remove destroyed players (those who are no longer in the scene)
        players.RemoveAll(player => player == null);
    }

    void CenterCamera()
    {
        // Move the camera to a default centered position
        Vector3 centeredPosition = Vector3.zero + offset; // You can adjust 'Vector3.zero' or the offset as needed
        centeredPosition.y = transform.position.y; // Keep the current Y position of the camera
        transform.position = Vector3.SmoothDamp(transform.position, centeredPosition, ref velocity, smoothTime);

        // Zoom out the camera to a specific value
        float customZoomOutSize = 20f; // Set your specific zoom-out value here
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, customZoomOutSize, Time.deltaTime);
    }
}
