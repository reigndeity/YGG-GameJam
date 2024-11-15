using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezer : MonoBehaviour
{
    public float slowSpeed;
    public float currentSpeed;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement movement = collision.gameObject.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                currentSpeed = movement.speed;
            }
        }
    }
    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement movement = collision.gameObject.GetComponent<PlayerMovement>();
            if(movement != null)
            {
                movement.speed = slowSpeed;
            }
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement movement = collision.gameObject.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                movement.speed = currentSpeed;
            }
        }
    }
}
