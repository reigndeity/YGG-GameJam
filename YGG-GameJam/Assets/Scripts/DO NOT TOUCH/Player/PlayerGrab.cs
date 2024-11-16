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
    
    
    [Header("Components")]
    private PlayerMovement _playerMovement;
    private Rigidbody _rigidbody;  // Rigidbody of the held object
    private IngredientBehavior _ingredientBehavior;

    private void Start()
    {
        _playerMovement = GetComponentInParent<PlayerMovement>();
    }
    

    public void GrabIngredient()
    {
        Vector3 rayOrigin = transform.position + new Vector3(0, -0.5f, 0);
        if (Physics.Raycast(rayOrigin, transform.forward, out RaycastHit hit, rayDistance, interactableLayer))
        {
            Grab(hit.collider.gameObject);
        }
            
    }

public void Grab(GameObject objectToGrab)
{
    grabbedObject = objectToGrab;
    _rigidbody = grabbedObject.GetComponent<Rigidbody>();
    grabbedObjectCollider = grabbedObject.GetComponent<MeshCollider>();
    grabbedBoxCollider = grabbedObject.GetComponent<BoxCollider>();
    _ingredientBehavior = grabbedObject.GetComponent<IngredientBehavior>();

    if (_rigidbody != null)
    {
        _rigidbody.useGravity = false;
        _rigidbody.isKinematic = true;

        _playerMovement.isCarrying = true;
        _playerMovement.canThrow = true;
    }

    if (grabbedObjectCollider != null)
    {
        grabbedObjectCollider.enabled = false;
        grabbedBoxCollider.enabled = false;
    }

    grabbedObject.transform.parent = holdPosition;
    grabbedObject.transform.localPosition = Vector3.zero;
    grabbedObject.transform.localRotation = Quaternion.identity;

    // Notify the IngredientBehavior script that the object has been grabbed
    _ingredientBehavior.OnGrabbed();
}

public void Release()
{
    if (_rigidbody != null)
    {
        _rigidbody.useGravity = true;
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);

        grabbedObject.transform.parent = null;
        grabbedObject = null;
        _rigidbody = null;
    }

    if (grabbedObjectCollider != null)
    {
        grabbedObjectCollider.enabled = true;
        grabbedBoxCollider.enabled = true;
        grabbedObjectCollider = null;
    }

    // Notify the IngredientBehavior script that the object has been released
    _ingredientBehavior.OnReleased();
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

    public void AdjustIngredientPosition()
    {
        if(grabbedObject == null) return;
        // BURGER ==================================================
        // Buns
        if (grabbedObject.tag == "Burger_Buns")
            {
                grabbedObject.transform.localPosition = Vector3.zero;
                grabbedObject.transform.localRotation = Quaternion.Euler(-14.74f, 0, 0);
            }
        // Cheese
        if (grabbedObject.tag == "Burger_Cheese")
            {
                grabbedObject.transform.localPosition = Vector3.zero;
                grabbedObject.transform.localRotation = Quaternion.Euler(-4.9f, 16.65f, 0);
            }
        // Patty
        if (grabbedObject.tag == "Burger_Patty")
            {
                grabbedObject.transform.localPosition = new Vector3(0, 0, 2f);
                grabbedObject.transform.localRotation = Quaternion.Euler(-18.86f,0, 0);
            }
        // HOTDOG ==================================================
        // Sausage
        if (grabbedObject.tag == "Hotdog_Sausage")
            {
                grabbedObject.transform.localPosition = new Vector3(0, 5.5f, -3f);
                grabbedObject.transform.localRotation = Quaternion.identity;
            }
        // Buns
        if (grabbedObject.tag == "Hotdog_Buns")
            {
                grabbedObject.transform.localPosition = new Vector3(0, 2.88f, 2.77f);
                grabbedObject.transform.localRotation = Quaternion.Euler(-116.27f,0, 0);
            }
        // Mustard
        if (grabbedObject.tag == "Hotdog_Mustard")
            {
                grabbedObject.transform.localPosition = new Vector3(-10.05f, 6.5f, 0.88f);
                grabbedObject.transform.localRotation = Quaternion.Euler(4.47f,85.3f, 0);
            }
        // SANDWHICH ==================================================
        // Bread
        if (grabbedObject.tag == "Sandwich_Bread")
            {
                grabbedObject.transform.localPosition = new Vector3(0.54f, 3.2f, 1.26f);
                grabbedObject.transform.localRotation = Quaternion.Euler(-116.27f,0, 0);
            }
        // Ham
        if (grabbedObject.tag == "Sandwich_Ham")
            {
                grabbedObject.transform.localPosition = new Vector3(0, 5.53f, 3.28f);
                grabbedObject.transform.localRotation = Quaternion.Euler(65f,0f, 0);
            }
        // Lettuce
        if (grabbedObject.tag == "Sandwich_Lettuce")
            {
                grabbedObject.transform.localPosition = new Vector3(-4.57f, 3.57f, 0f);
                grabbedObject.transform.localRotation = Quaternion.Euler(65f,0f, 0);
            }
    }
}
