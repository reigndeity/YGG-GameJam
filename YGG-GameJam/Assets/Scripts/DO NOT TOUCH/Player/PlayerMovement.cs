using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Properties")]
    [SerializeField] private int playerID = 1; // Player ID (1, 2, 3, or 4)
    public float speed = 5f; // Speed of the player movement
    [SerializeField] private float rotationSpeed = 10f; // Speed of rotation towards the movement direction
    [SerializeField] private float jumpForce;
    [SerializeField] private GameObject pushObject;

    [Header("----------------")]
    public bool isGrabbing;
    public bool isCarrying;
    public bool canThrow;
    public bool canPush;
    public bool isGrounded;

    [Header("Control Properties")]
    [SerializeField] private bool isKeyboard;  // Toggle to use keyboard or controller
    public KeyCode keyCodeOne;
    public KeyCode keyCodeTwo;
    public KeyCode keyCodeThree;
    public KeyCode keyCodeFour;

    [Header("Components")]
    private Animator _animator;
    private PlayerJump _playerJump;
    private PlayerGrab _playerGrab;
    private GamepadManager gamepadManager;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerJump = GetComponentInChildren<PlayerJump>();
        _playerGrab = GetComponentInChildren<PlayerGrab>();
        gamepadManager = FindObjectOfType<GamepadManager>(); // Find GamepadManager in the scene
        canPush = true;
    }

    private void Update()
    {
        // Decide input method based on GamepadManager assignment
        bool useController = gamepadManager != null && gamepadManager.IsControllerAssignedToPlayer(playerID);
        
        Vector3 moveDirection = Vector3.zero;
        
        if (useController)
        {
            // GAMEPAD CONTROLS =================================
            float horizontalInput = Input.GetAxis("Horizontal" + playerID);
            float verticalInput = Input.GetAxis("Vertical" + playerID);
            moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
            if (horizontalInput != 0 || verticalInput != 0)
            {
                Debug.Log("Player " + playerID + " is moving with controller input.");
            }
            // Controller Button Inputs
            if (Input.GetButtonDown("Jump" + playerID) && isGrounded && !isCarrying && !isGrabbing && canPush) // "Y" button
            {
                Debug.Log("Player " + playerID + " Y button (controller) pressed");
                _playerJump.Jump();
            }
            if (Input.GetButtonDown("Fire1_" + playerID) && isGrounded && !isGrabbing && canPush) // "A" button
            {
                Debug.Log("Player " + playerID + " A button (controller) pressed");
                if (!isCarrying)
                {
                    isGrabbing = true;
                }
                else
                {
                    canThrow = false;
                } 
            }
            if (Input.GetButtonDown("Fire2_" + playerID) && isGrounded && !isGrabbing && !isCarrying && canPush) // "B" button
            {
                Debug.Log("Player " + playerID + " B button (controller) pressed");
                canPush = false;
            }
            if (Input.GetButtonDown("Fire3_" + playerID)) // "X" button
            {
                Debug.Log("Player " + playerID + " X button (controller) pressed");
            }
        }
        else if (isKeyboard)
        {
            // KEYBOARD CONTROLS =================================
            float horizontalKeyboard = Input.GetAxis("HorizontalKeyboard");
            float verticalKeyboard = Input.GetAxis("VerticalKeyboard");
            moveDirection = new Vector3(horizontalKeyboard, 0, verticalKeyboard).normalized;

            if (Input.GetKeyDown(keyCodeOne) && isGrounded && !isCarrying && !isGrabbing && canPush)
            {
                Debug.Log(keyCodeOne + " : Y button pressed");
                _playerJump.Jump();
            }
            if (Input.GetKeyDown(keyCodeTwo) && isGrounded && !isGrabbing && canPush)
            {
                Debug.Log(keyCodeTwo + " : A button pressed");
                if (!isCarrying)
                {
                    isGrabbing = true;
                }
                else
                {
                    canThrow = false;
                }
            }
            if (Input.GetKeyDown(keyCodeThree) && isGrounded && !isGrabbing && !isCarrying && canPush)
            {
                Debug.Log(keyCodeThree + " : B button pressed");
                canPush = false;
            }
            if (Input.GetKeyDown(keyCodeFour))
            {
                Debug.Log(keyCodeFour + " : X button pressed");
            }
        }
        
        // Apply movement
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        // ANIMATOR =================================
        UpdateAnimator(moveDirection);


        
    }

    private void UpdateAnimator(Vector3 moveDirection)
    {
        if (moveDirection == Vector3.zero && isGrounded && !isCarrying && !isGrabbing && canPush) // IDLE
        {
            _animator.SetInteger("animState", 0);
        }
        if (moveDirection != Vector3.zero && isGrounded && !isCarrying && !isGrabbing && canPush) // RUNNING
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            _animator.SetInteger("animState", 1);
        }
        if (moveDirection == Vector3.zero && !isGrounded && !isCarrying && !isGrabbing) // STATIONARY JUMP
        {
            _animator.SetInteger("animState", 2);
        }
        if (moveDirection != Vector3.zero && !isGrounded && !isCarrying && !isGrabbing) // MOVING WHILE JUMPING
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            _animator.SetInteger("animState", 2);
        }
        if (moveDirection == Vector3.zero && isGrounded && !isCarrying && isGrabbing) // IDLE GRABBING
        {
            _animator.SetInteger("animState", 3);
        }
        if (moveDirection != Vector3.zero && isGrounded && !isCarrying && isGrabbing) // MOVING WHILE GRABBING
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            _animator.SetInteger("animState", 3);
        }
        if (moveDirection == Vector3.zero && isGrounded && isCarrying && !isGrabbing && canThrow) // IDLE CARRYING
        {
            _animator.SetInteger("animState", 4);
        }
        if (moveDirection != Vector3.zero && isGrounded && isCarrying && !isGrabbing && canThrow) // MOVING WHILE CARRYING
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            _animator.SetInteger("animState", 5);
        }
        if (moveDirection == Vector3.zero && isGrounded && isCarrying && !isGrabbing && !canThrow) // IDLE THROWING
        {
            _animator.SetInteger("animState", 6);
        }
        if (moveDirection != Vector3.zero && isGrounded && isCarrying && !isGrabbing && !canThrow) // MOVING WHILE THROWING
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            _animator.SetInteger("animState", 6);
        }
        if (moveDirection == Vector3.zero && isGrounded && !isCarrying && !isGrabbing && !canPush) // STATIONARY PUSH
        {
            _animator.SetInteger("animState", 7);
        }
        if (moveDirection != Vector3.zero && isGrounded && !isCarrying && !isGrabbing && !canPush) // MOVING WHILE PUSHING
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            _animator.SetInteger("animState", 7);
        }
    }

    // UNITY EVENT SYSTEM METHODS
    public void ResetGrab() => isGrabbing = false;
    public void ResetCarrying() => isCarrying = false;
    public void GrabItem()
    {
        _playerGrab.GrabIngredient();
        AdjustItemPosition();
    } 
    public void ReleaseItem() => _playerGrab.Release();
    public void ReleaseIngredient()
    {
        isGrounded = true;
        isCarrying = true;
        isGrabbing = false;
        canThrow = false;
    }
    public void ActivatePush() => pushObject.SetActive(true);
    public void DeactivatePush()
    {
        pushObject.SetActive(false);
        canPush = true;
    }
    public void AdjustItemPosition()
    {
        _playerGrab.AdjustIngredientPosition();
    }
}
