using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public Rigidbody _rigidbody;
    [SerializeField] float jumpForce;
    public PlayerMovement _playerMovement;


    void Start()
    {
        _rigidbody = GetComponentInParent<Rigidbody>();
        _playerMovement = GetComponentInParent<PlayerMovement>();
    }

    public void Jump()
    {
        if (_playerMovement.isGrounded == true)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, jumpForce, _rigidbody.velocity.z);
            _playerMovement.isGrounded = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            _playerMovement.isGrounded = true;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            _playerMovement.isGrounded = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            _playerMovement.isGrounded = false;
        }
    }
}
