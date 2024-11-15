using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    public float pushForce;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Pushed");

            Rigidbody _targetRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            PlayerMovement _playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            PlayerGrab _playerGrab = collision.gameObject.GetComponentInChildren<PlayerGrab>();

            // Calculate the push direction based on where the player is facing
            //Vector3 pushDirection = (targetRigidbody.transform.position - transform.position).normalized;
            _targetRigidbody.AddForce(transform.forward * pushForce, ForceMode.VelocityChange);
            _playerMovement.ReleaseIngredient(); 
            _playerGrab.Release();
        }
        
    }
}
