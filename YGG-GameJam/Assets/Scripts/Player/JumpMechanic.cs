using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMechanic : MonoBehaviour
{
    public Rigidbody _rigidbody;
    public bool isGrounded;
    [SerializeField] float jumpForce;

    void Start()
    {
        _rigidbody = GetComponentInParent<Rigidbody>();
    }

    public void Jump()
    {
        if (isGrounded == true)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, jumpForce, _rigidbody.velocity.z);
            isGrounded = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            isGrounded = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}
