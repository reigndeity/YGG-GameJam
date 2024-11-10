using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
 public Transform[] players; // Array to store all player transforms
    public float minZoom = 5f;  // Closest zoom level
    public float maxZoom = 10f; // Farthest zoom level
    public float minDistance = 2f;  // Minimum distance for minZoom
    public float maxDistance = 10f; // Maximum distance for maxZoom
    public float smoothSpeed = 0.125f; // Smoothness for zoom and position transitions
    public Vector3 offset = new Vector3(0, 25.9f, -30f);  // Adjust based on isometric angle

    private Camera cam;

    void Start()
    {
        cam = Camera.main; // Get the main camera
    }

    void LateUpdate()
    {
        if (players.Length == 0) return;

        // Step 1: Calculate maximum distance between players
        float maxDistance = CalculateMaxDistance();

        // Step 2: Calculate desired zoom based on max distance
        float targetZoom = Mathf.Lerp(minZoom, maxZoom, Mathf.InverseLerp(minDistance, maxDistance, maxDistance));
        
        // Step 3: Find the center point of all players
        Vector3 centerPosition = GetCenterPoint();

        // Step 4: Smoothly move and zoom the camera
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, smoothSpeed);
        cam.transform.position = Vector3.Lerp(cam.transform.position, centerPosition + offset, smoothSpeed);
    }

    float CalculateMaxDistance()
    {
        float maxDistance = 0f;
        for (int i = 0; i < players.Length; i++)
        {
            for (int j = i + 1; j < players.Length; j++)
            {
                float distance = Vector3.Distance(players[i].position, players[j].position);
                if (distance > maxDistance)
                    maxDistance = distance;
            }
        }
        return maxDistance;
    }

    Vector3 GetCenterPoint()
    {
        Vector3 center = Vector3.zero;
        foreach (Transform player in players)
        {
            center += player.position;
        }
        return center / players.Length;
    }
}
