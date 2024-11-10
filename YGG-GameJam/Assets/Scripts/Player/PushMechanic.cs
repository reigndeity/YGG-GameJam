using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushMechanic : MonoBehaviour
{
    public float pushForce;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody targetRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            // Calculate the push direction based on where the player is facing
            Vector3 pushDirection = (targetRigidbody.transform.position - transform.position).normalized;
            targetRigidbody.AddForce(pushDirection * pushForce);
        }
        
    }
}
