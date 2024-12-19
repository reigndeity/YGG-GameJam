using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public Rigidbody _rigidbody;
    [SerializeField] float jumpForce; // Increased default value for faster jump
    [SerializeField] float extraGravity; // Additional gravity for faster descent
    public PlayerMovement _playerMovement;

    void Start()
    {
        _rigidbody = GetComponentInParent<Rigidbody>();
        _playerMovement = GetComponentInParent<PlayerMovement>();

        jumpForce = 15;
        extraGravity = 30;
    }

    public void Jump()
    {
        Debug.Log(jumpForce);
        if (_playerMovement.isGrounded)
        {
            _rigidbody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            _playerMovement.isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        if (!_playerMovement.isGrounded)
        {
            _rigidbody.AddForce(Vector3.down * extraGravity, ForceMode.Acceleration);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground" || other.gameObject.layer == 3)
        {
            _playerMovement.isGrounded = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Ground" || other.gameObject.layer == 3)
        {
            _playerMovement.isGrounded = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ground" || other.gameObject.layer == 3)
        {
            _playerMovement.isGrounded = false;
        }
    }
}
