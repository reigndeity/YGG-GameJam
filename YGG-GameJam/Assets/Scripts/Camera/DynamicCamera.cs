using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
    public Transform[] players; // Array to hold the players' transforms
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
        if (players.Length == 0) return;

        MoveCamera();
        ZoomCamera();
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
        if (players.Length == 1)
        {
            return players[0].position;
        }

        // Calculate the bounding box that encapsulates all players
        var bounds = new Bounds(players[0].position, Vector3.zero);
        for (int i = 1; i < players.Length; i++)
        {
            bounds.Encapsulate(players[i].position);
        }

        return bounds.center;
    }

    float GetGreatestDistance()
    {
        // Calculate the bounding box that encapsulates all players
        var bounds = new Bounds(players[0].position, Vector3.zero);
        for (int i = 1; i < players.Length; i++)
        {
            bounds.Encapsulate(players[i].position);
        }

        // Return the largest dimension of the bounding box
        return bounds.size.x > bounds.size.z ? bounds.size.x : bounds.size.z;
    }
}