using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushMechanic : MonoBehaviour
{
    public float pushForce;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Pushed");

            Rigidbody targetRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            GrabMechanic targetGrabMechanic = collision.gameObject.GetComponent<GrabMechanic>();

            // Calculate the push direction based on where the player is facing
            //Vector3 pushDirection = (targetRigidbody.transform.position - transform.position).normalized;
            targetRigidbody.AddForce(transform.forward * pushForce, ForceMode.VelocityChange);
            targetGrabMechanic.Release();
        }
        
    }
}
