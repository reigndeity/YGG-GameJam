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

    private void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        currentSpeed = playerController.speed;
        slowedSpeed = playerController.speed - 1.5f;
    }
    

    public void GrabIngredient()
    {
        Vector3 rayOrigin = transform.position + new Vector3(0, -0.5f, 0);
        if (Physics.Raycast(rayOrigin, transform.forward, out RaycastHit hit, rayDistance, interactableLayer))
            Grab(hit.collider.gameObject);
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
        }

        if (grabbedObjectCollider != null)
        {
            // Re-enable the collider when released
            grabbedObjectCollider.enabled = true;
        }

        // Clear references and reset parent
        grabbedObject.transform.parent = null;
        grabbedObject = null;
        grabbedObjectRb = null;
        grabbedObjectCollider = null;

        playerController.speed = currentSpeed;
    }

    // Method to draw the ray in the Scene view
    void OnDrawGizmos()
    {
        // Set the Gizmo color to blue for visibility
        Gizmos.color = Color.blue;

        // Draw a ray from the player's position in the forward direction, up to the rayDistance
        Vector3 rayOrigin = transform.position + new Vector3(0, -0.5f, 0);
        Gizmos.DrawRay(rayOrigin, transform.forward * rayDistance);

        // Optional: Draw a sphere at the end of the ray to indicate the maximum reach
        Gizmos.DrawWireSphere(rayOrigin + transform.forward * rayDistance, 0.2f);
    } 
}
