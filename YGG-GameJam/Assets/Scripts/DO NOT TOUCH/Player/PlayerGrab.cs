using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{

    [Header("Editable Properties")]
    [SerializeField] float rayDistance = 5f;      // The maximum distance for the raycast
    [SerializeField] float throwForce = 8f; // Adjust this value as needed for throw strength
    [SerializeField] LayerMask interactableLayer; // Layer of the interactable objects
    [SerializeField] Transform holdPosition;      // The position where the object will be held

    [Header("Display Properties")]
    [SerializeField] GameObject grabbedObject;    // The object currently being held
    private MeshCollider grabbedObjectCollider; // MeshCollider of the held object
    private BoxCollider grabbedBoxCollider;
    
    [SerializeField] float currentSpeed;
    [SerializeField] float slowedSpeed;
    
    [Header("Components")]
    private PlayerMovement _playerMovement;
    private Rigidbody _rigidbody;  // Rigidbody of the held object

    private void Start()
    {
        _playerMovement = GetComponentInParent<PlayerMovement>();
        currentSpeed = _playerMovement.speed;
        slowedSpeed = _playerMovement.speed - 1.5f;
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
        _rigidbody = grabbedObject.GetComponent<Rigidbody>();
        grabbedObjectCollider = grabbedObject.GetComponent<MeshCollider>();
        grabbedBoxCollider = grabbedObject.GetComponent<BoxCollider>();
        if (_rigidbody != null)
        {
            // Disable physics for holding
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;

            _playerMovement.isCarrying = true;
            _playerMovement.canThrow = true;
            _playerMovement.speed = slowedSpeed;
        }

        if (grabbedObjectCollider != null)
        {
            // Disable the collider for holding
            grabbedObjectCollider.enabled = false;
            grabbedBoxCollider.enabled = false;
        }

        // Set the object's parent to the hold position
        grabbedObject.transform.parent = holdPosition;

        // Lock the local position and rotation to zero relative to holdPosition
        grabbedObject.transform.localPosition = Vector3.zero;
        grabbedObject.transform.localRotation = Quaternion.identity;
    }

    public void Release()
    {
        if (_rigidbody != null)
        {
            // Re-enable physics for release
            _rigidbody.useGravity = true;
            _rigidbody.isKinematic = false;

            // Apply a forward force to simulate a throw
            _rigidbody.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);

            grabbedObject.transform.parent = null;
            grabbedObject = null;
            _rigidbody = null;
        }

        if (grabbedObjectCollider != null)
        {
            // Re-enable the collider when released
            grabbedObjectCollider.enabled = true;
            grabbedBoxCollider.enabled = true;
            grabbedObjectCollider = null;
        }

        // Clear references and reset parent
        /*grabbedObject.transform.parent = null;
        grabbedObject = null;
        grabbedObjectRb = null;
        grabbedObjectCollider = null;*/

        _playerMovement.speed = currentSpeed;
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
