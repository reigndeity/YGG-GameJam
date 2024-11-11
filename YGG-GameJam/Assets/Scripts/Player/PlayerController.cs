using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Properties")]
    public float speed = 5f; // Speed of the player movement
    [SerializeField] private int playerID = 1; // Player ID (1, 2, 3, or 4)
    [SerializeField] private bool isKeyboard;  // Toggle to use keyboard or controller
    [SerializeField] private float rotationSpeed = 10f; // Speed of rotation towards the movement direction
    public bool canPush;
    public GameObject pushObject; //for pushing object only. change this to animator

    [Header("Animator Properties")]
    [SerializeField] bool isGrabbing;
    public bool isCarrying;
    public bool canThrow;

    [Header("Script References")]
    public GrabMechanic _grabMechanic;
    public JumpMechanic _jumpMechanic;
    private Animator _animator;

    void Start()
    {
        _grabMechanic = GetComponentInChildren<GrabMechanic>();
        _jumpMechanic = GetComponentInChildren<JumpMechanic>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 moveDirection = Vector3.zero;

        if (isKeyboard)
        {
            // Keyboard Movement (WASD)
            float horizontalKeyboard = Input.GetAxis("HorizontalKeyboard");
            float verticalKeyboard = Input.GetAxis("VerticalKeyboard");
            moveDirection = new Vector3(horizontalKeyboard, 0, verticalKeyboard).normalized;

            // Keyboard Button Inputs (HJKL for AXBY)
            if (Input.GetButtonDown("Keyboard_A") && _jumpMechanic.isGrounded == true) // "H" key
            {
                Debug.Log("Keyboard A button (H key) pressed");
                if (canThrow == false && isCarrying == false)
                {
                    isGrabbing = true;
                    _animator.SetInteger("animState", 3);
                }
                if (canThrow == true && isCarrying == true)
                {
                    ReleaseIngredient();
                    Debug.Log("player is throwing");
                }
            }

            if (Input.GetButtonDown("Keyboard_X") && canPush) // "J" key
            {
                Debug.Log("Keyboard X button (J key) pressed");

                canPush = false;
                _animator.SetInteger("animState", 7);
            }

            if (Input.GetButtonDown("Keyboard_B")) // "K" key
            {
                Debug.Log("Keyboard B button (K key) pressed");
            }

            if (Input.GetButtonDown("Keyboard_Y")) // "L" key
            {
                Debug.Log("Keyboard Y button (L key) pressed");
                _jumpMechanic.Jump();
                _animator.SetInteger("animState", 2);
            }
        }
        else // Joystick Movement (Controller)===============================
        {

            float horizontalInput = Input.GetAxis("Horizontal" + playerID);
            float verticalInput = Input.GetAxis("Vertical" + playerID);
            moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

            // Controller Button Inputs
            if (Input.GetButtonDown("Jump" + playerID) && isCarrying == false) // "Y" button
            {
                Debug.Log("Player " + playerID + " Y button (controller) pressed");
                _jumpMechanic.Jump();
                _animator.SetInteger("animState", 2);
            }

            if (Input.GetButtonDown("Fire1_" + playerID) && _jumpMechanic.isGrounded == true) // "A" button
            {
                Debug.Log("Player " + playerID + " A button (controller) pressed");
                if (canThrow == false && isCarrying == false)
                {
                    isGrabbing = true;
                    _animator.SetInteger("animState", 3);
                }
                if (canThrow == true && isCarrying == true)
                {
                    ReleaseIngredient();
                    Debug.Log("player is throwing");
                }

            }
            if (Input.GetButtonDown("Fire2_" + playerID)) // "B" button
            {
                Debug.Log("Player " + playerID + " B button (controller) pressed");
            }

            if (Input.GetButtonDown("Fire3_" + playerID)) // "X" button
            {
                Debug.Log("Player " + playerID + " X button (controller) pressed");
            }
        }

        // Move the player based on the selected input method
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        // Idle
        if (moveDirection == Vector3.zero && _jumpMechanic.isGrounded == true && isGrabbing == false && isCarrying == false && canPush)
        {
            _animator.SetInteger("animState", 0);
        }
        // Running
        if (moveDirection != Vector3.zero && _jumpMechanic.isGrounded == true && isGrabbing == false && isCarrying == false && canPush)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            _animator.SetInteger("animState", 1);
        }
        // Jump while Running
        if (moveDirection != Vector3.zero && _jumpMechanic.isGrounded == false)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            _animator.SetInteger("animState", 2);
        }
        // Grabbing while Running
        if (moveDirection != Vector3.zero && _jumpMechanic.isGrounded == true && isGrabbing == true && isCarrying == false && canPush)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            _animator.SetInteger("animState", 3);
        }
        // Idle with Item
        if (moveDirection == Vector3.zero && _jumpMechanic.isGrounded == true && isGrabbing == false && isCarrying == true && canPush)
        {
            if (canThrow == true)
            {
                _animator.SetInteger("animState", 4);
            }
            else
            {
                _animator.SetInteger("animState", 5);
            }
        }
        // Running with Item
        if (moveDirection != Vector3.zero && _jumpMechanic.isGrounded == true && isGrabbing == false && isCarrying == true && canThrow == true && canPush)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            _animator.SetInteger("animState", 6);
        }
    }

    // UNITY EVENT SYSTEM: DO NOT ALTER/CHANGE
    public void IsGrabbingReset()
    {
        isGrabbing = false;
    }
    public void CanThrowReset()
    {

        isCarrying = false;
        _grabMechanic.Release();
    }

    public void GrabIngredient()
    {
        if (_grabMechanic.grabbedObject == null && canThrow == false)
        {
            _grabMechanic.GrabIngredient();
        }
    }
    public void ReleaseIngredient()
    {
        if (_grabMechanic.grabbedObject != null && canThrow == true)
        {
            _animator.SetInteger("animState", 5);
            canThrow = false;
        }
    }

    public void ShowPush() 
    { 
        pushObject.SetActive(true);
    }
    public void DeactivatePush()
    {
        pushObject.SetActive(false);
        canPush = true;
    }
}
