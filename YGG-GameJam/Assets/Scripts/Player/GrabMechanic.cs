using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabMechanic : MonoBehaviour
{
    public float rayDistance = 5f;      // The maximum distance for the raycast
    public LayerMask interactableLayer; // Layer of the interactable objects
    public Transform holdPosition;      // The position where the object will be held
    public GameObject grabbedObject;    // The object currently being held
    private Rigidbody grabbedObjectRb;  // Rigidbody of the held object
    private MeshCollider grabbedObjectCollider; // MeshCollider of the held object
    public PlayerController playerController;
    public float currentSpeed;
    public float slowedSpeed;
    public float throwForce = 8f; // Adjust this value as needed for throw strength

    [Header("Raycast Variables")]
    public float grabRadius = 1f;
    public float grabRange = 2f;
    public float grabAngle = 30f;

    private void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        currentSpeed = playerController.speed;
        slowedSpeed = playerController.speed - 1.5f;
    }

    //Original GrabIngredient Code
    /*public void GrabIngredient()
    {
        Vector3 rayOrigin = transform.position + new Vector3(0, -0.5f, 0);
        if (Physics.Raycast(rayOrigin, transform.forward, out RaycastHit hit, rayDistance, interactableLayer))
            Grab(hit.collider.gameObject);
    }*/

    public void GrabIngredient()
    {
        Vector3 rayOrigin = transform.position + new Vector3(0, -0.5f, 0);
        RaycastHit hit;

        // SphereCast to detect objects within grabRadius along forward direction
        if (Physics.SphereCast(rayOrigin, grabRadius, transform.forward, out hit, grabRange, interactableLayer))
        {
            // Check if the object is within the allowed grab angle
            Vector3 toTarget = (hit.collider.transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, toTarget) <= grabAngle)
            {
                Grab(hit.collider.gameObject);
            }
        }
    }


    public void Grab(GameObject objectToGrab)
    {
        grabbedObject = objectToGrab;
        grabbedObjectRb = grabbedObject.GetComponent<Rigidbody>();
        grabbedObjectCollider = grabbedObject.GetComponent<MeshCollider>();
        if (grabbedObjectRb != null)
        {
            // Disable physics for holding
            grabbedObjectRb.useGravity = false;
            grabbedObjectRb.isKinematic = true;

            playerController.isCarrying = true;
            playerController.canThrow = true;
            playerController.speed = slowedSpeed;
        }

        if (grabbedObjectCollider != null)
        {
            // Disable the collider for holding
            grabbedObjectCollider.enabled = false;
        }

        // Set the object's parent to the hold position
        grabbedObject.transform.parent = holdPosition;

        // Lock the local position and rotation to zero relative to holdPosition
        grabbedObject.transform.localPosition = Vector3.zero;
        grabbedObject.transform.localRotation = Quaternion.identity;
    }

    public void Release()
    {
        if (grabbedObjectRb != null)
        {
            // Re-enable physics for release
            grabbedObjectRb.useGravity = true;
            grabbedObjectRb.isKinematic = false;

            // Apply a forward force to simulate a throw
            grabbedObjectRb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);

            grabbedObject.transform.parent = null;
            grabbedObject = null;
            grabbedObjectRb = null;
        }

        if (grabbedObjectCollider != null)
        {
            // Re-enable the collider when released
            grabbedObjectCollider.enabled = true;

            grabbedObjectCollider = null;
        }

        playerController.speed = currentSpeed;
    }

    // Method to draw the ray in the Scene view
    //Original Gizmo Code
    /*void OnDrawGizmos()
    {
        // Set the Gizmo color to blue for visibility
        Gizmos.color = Color.blue;

        // Draw a ray from the player's position in the forward direction, up to the rayDistance
        Vector3 rayOrigin = transform.position + new Vector3(0, -0.5f, 0);
        Gizmos.DrawRay(rayOrigin, transform.forward * rayDistance);

        // Optional: Draw a sphere at the end of the ray to indicate the maximum reach
        Gizmos.DrawWireSphere(rayOrigin + transform.forward * rayDistance, 0.2f);
    } */

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Vector3 rayOrigin = transform.position + new Vector3(0, -0.5f, 0);
        Vector3 rayDirection = transform.forward * grabRange;

        // Draw the initial sphere at the start position
        Gizmos.DrawWireSphere(rayOrigin, grabRadius);

        // Draw the line representing the cast direction
        Gizmos.DrawRay(rayOrigin, rayDirection);

        // Draw the sphere at the end of the cast range
        Gizmos.DrawWireSphere(rayOrigin + rayDirection, grabRadius);
    }

}
