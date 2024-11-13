using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Properties")]
    [SerializeField] private int playerID = 1; // Player ID (1, 2, 3, or 4)
    public float speed = 5f; // Speed of the player movement
    [SerializeField] private float rotationSpeed = 10f; // Speed of rotation towards the movement direction
    [SerializeField] float jumpForce;
    [SerializeField] GameObject pushObject;

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
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerJump = GetComponentInChildren<PlayerJump>();
        _playerGrab = GetComponentInChildren<PlayerGrab>();
        canPush = true;
    }

    private void Update()
    {
        // Move the player based on the selected input method
        Vector3 moveDirection = Vector3.zero;
        if (isKeyboard)
        {
            // Keyboard Movement (WASD)
            float horizontalKeyboard = Input.GetAxis("HorizontalKeyboard");
            float verticalKeyboard = Input.GetAxis("VerticalKeyboard");
            moveDirection = new Vector3(horizontalKeyboard, 0, verticalKeyboard).normalized;

            // KEYBOARD CONTROLS =================================
            if (Input.GetKeyDown(keyCodeOne) && isGrounded == true && isCarrying == false && isGrabbing == false && canPush == true)
            {
                Debug.Log(keyCodeOne + " : Y button pressed");
                _playerJump.Jump();
            }

            if (Input.GetKeyDown(keyCodeTwo) && isGrounded == true && isGrabbing == false && canPush == true)
            {
                Debug.Log(keyCodeTwo + " : A button pressed");
                if (isCarrying == false)
                {
                    isGrabbing = true;
                }
                else
                {
                    canThrow = false;
                }
            }

            if (Input.GetKeyDown(keyCodeThree) && isGrounded == true && isGrabbing == false && isCarrying == false && canPush == true)
            {
                Debug.Log(keyCodeThree +" : B button pressed");
                canPush = false;
            }
            if (Input.GetKeyDown(keyCodeFour))
            {
                Debug.Log(keyCodeFour +" : X button pressed");
            }
        }
        else // GAMEPAD CONTROLS =================================
        {

            float horizontalInput = Input.GetAxis("Horizontal" + playerID);
            float verticalInput = Input.GetAxis("Vertical" + playerID);
            moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

            // Controller Button Inputs
            if (Input.GetButtonDown("Jump" + playerID) && isGrounded == true && isCarrying == false && isGrabbing == false && canPush == true) // "Y" button
            {
                Debug.Log("Player " + playerID + " Y button (controller) pressed");
                _playerJump.Jump();
            }
            if (Input.GetButtonDown("Fire1_" + playerID) && isGrounded == true && isGrabbing == false && canPush == true) // "A" button
            {
                Debug.Log("Player " + playerID + " A button (controller) pressed");
                if (isCarrying == false)
                {
                    isGrabbing = true;
                }
                else
                {
                    canThrow = false;
                } 
            }
            if (Input.GetButtonDown("Fire2_" + playerID) && isGrounded == true && isGrabbing == false && isCarrying == false && canPush == true) // "B" button
            {
                Debug.Log("Player " + playerID + " B button (controller) pressed");
                canPush = false;
            }
            if (Input.GetButtonDown("Fire3_" + playerID)) // "X" button
            {
                Debug.Log("Player " + playerID + " X button (controller) pressed");
            }
        }
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        //ANIMATOR =================================
        if (moveDirection == Vector3.zero && isGrounded == true && isCarrying == false && isGrabbing == false && canPush == true) // IDLE : Not moving, not airborne, not carrying an item, not grabbing, can push.
        {
            _animator.SetInteger("animState", 0);
        }
        if (moveDirection != Vector3.zero && isGrounded == true && isCarrying == false && isGrabbing == false && canPush == true) // RUNNING : Moving, not airborne, not carrying an item, not grabbing, can push.
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            _animator.SetInteger("animState", 1);
        }
        if (moveDirection == Vector3.zero && isGrounded == false && isCarrying == false && isGrabbing == false) // STATIONARY JUMP : Not moving, airborne, not carrying an item, not grabbing.
        {
            _animator.SetInteger("animState", 2);
        }
        if (moveDirection != Vector3.zero && isGrounded == false && isCarrying == false && isGrabbing == false) // MOVING WHILE JUMPING : Moving, airborne, not carrying an item, not grabbing.
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            _animator.SetInteger("animState", 2);
        }
        if (moveDirection == Vector3.zero && isGrounded == true && isCarrying == false && isGrabbing == true) // IDLE GRABING : Not moving, not airborne, not carrying an item, grabbing.
        {
            _animator.SetInteger("animState", 3);
        }
        if (moveDirection != Vector3.zero && isGrounded == true && isCarrying == false && isGrabbing == true) // MOVING WHILE GRABBING : Moving, not airborne, not carrying an item, grabbing.
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            _animator.SetInteger("animState", 3);
        }
        if (moveDirection == Vector3.zero && isGrounded == true && isCarrying == true && isGrabbing == false && canThrow == true) // IDLE CARRYING AN ITEM : Not moving, not airborne, carrying an item, not grabbing, can throw.
        {
            _animator.SetInteger("animState", 4);
        }
        if (moveDirection != Vector3.zero && isGrounded == true && isCarrying == true && isGrabbing == false && canThrow == true) // MOVING WHILE CARRYING AN ITEM : Moving, not airborne, carrying an item, not grabbing, can throw.
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            _animator.SetInteger("animState", 5);
        }
        if (moveDirection == Vector3.zero && isGrounded == true && isCarrying == true && isGrabbing == false && canThrow == false) // IDLE THROWING AN ITEM : Not moving, not airborne, not carrying an item, not grabbing, is throwing.
        {
            _animator.SetInteger("animState", 6);
        }
        if (moveDirection != Vector3.zero && isGrounded == true && isCarrying == true && isGrabbing == false && canThrow == false) // MOVING WHILE THROWING AN ITEM : Moving, not airborne, not carrying an item, not grabbing, is throwing.
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            _animator.SetInteger("animState", 6);
        }
        if (moveDirection == Vector3.zero && isGrounded == true && isCarrying == false && isGrabbing == false && canPush == false) // STATIONARY PUSH : Not moving, not airborne, not carrying an item, not grabbing.
        {
            _animator.SetInteger("animState", 7);
        }
        if (moveDirection != Vector3.zero && isGrounded == true && isCarrying == false && isGrabbing == false && canPush == false) // MOVING WHILE PUSHING : Moving, not airborne, not carrying an item, not grabbing.
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            _animator.SetInteger("animState", 7);
        }
    }


    // UNITY EVENT SYSTEM
    public void ResetGrab()
    {
        isGrabbing = false;
    }
    public void ResetCarrying()
    {
        isCarrying = false;
    }
    public void GrabItem()
    {
        _playerGrab.GrabIngredient();
    }
    public void ReleaseItem()
    {
        _playerGrab.Release();
    }
    public void ReleaseIngredient()
    {
        isGrounded = true; 
        isCarrying = true; 
        isGrabbing = false; 
        canThrow = false;
    }
    public void ActivatePush() 
    { 
        pushObject.SetActive(true);
    }
    public void DeactivatePush()
    {
        pushObject.SetActive(false);
        canPush = true;
    }
}

