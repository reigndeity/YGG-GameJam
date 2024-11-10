using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabMechanics : MonoBehaviour
{
    public float rayDistance = 5f;      // The maximum distance for the raycast
    public LayerMask interactableLayer; // Layer of the interactable objects
    public Transform holdPosition;      // The position where the object will be held
    public GameObject grabbedObject;   // The object currently being held
    private Rigidbody grabbedObjectRb;  // Rigidbody of the held object
    private PlayerController playerController;
    public float currentSpeed;
    public float slowedSpeed;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        currentSpeed = playerController.speed;
        slowedSpeed = playerController.speed - 1.5f;
    }
    void Update()
    {
        if (grabbedObject == null)
        {
            playerController.speed = currentSpeed; //Normal player speed if not holding anything

            // Check for an object to grab
            Vector3 rayOrigin = transform.position + new Vector3(0, -0.5f, 0);
            if (Physics.Raycast(rayOrigin, transform.forward, out RaycastHit hit, rayDistance, interactableLayer))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Grab(hit.collider.gameObject);
                }
            }
        }
        else
        {
            playerController.speed = slowedSpeed; //Slows player if holding an ingredient

            // If already holding an object, check to release it
            if (Input.GetKeyDown(KeyCode.E))
            {
                Release();
            }
            else
            {
                // Keep the grabbed object at the hold position
                grabbedObject.transform.position = holdPosition.position;
            }
        }
    }

    public void Grab(GameObject objectToGrab)
    {
        grabbedObject = objectToGrab;
        grabbedObjectRb = grabbedObject.GetComponent<Rigidbody>();

        if (grabbedObjectRb != null)
        {
            // Disable physics for holding
            grabbedObjectRb.useGravity = false;
            grabbedObjectRb.isKinematic = true;
        }

        // Set the object's position to the hold position
        grabbedObject.transform.position = holdPosition.position;
        grabbedObject.transform.parent = holdPosition;
    }

    public void Release()
    {
        if (grabbedObjectRb != null)
        {
            // Re-enable physics for release
            grabbedObjectRb.useGravity = true;
            grabbedObjectRb.isKinematic = false;
        }

        // Clear references and reset parent
        grabbedObject.transform.parent = null;
        grabbedObject = null;
        grabbedObjectRb = null;
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

    public void GrabIngredient()
    {
        Vector3 rayOrigin = transform.position + new Vector3(0, -0.5f, 0);
        Physics.Raycast(rayOrigin, transform.forward, out RaycastHit hit, rayDistance, interactableLayer);
        Grab(hit.collider.gameObject);
    }
}
