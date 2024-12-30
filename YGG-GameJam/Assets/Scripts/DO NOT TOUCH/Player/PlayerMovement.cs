using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Properties")]
    [SerializeField] private int playerID = 1; // Player ID (1, 2, 3, or 4)
    public float speed = 5f; // Speed of the player movement
    [SerializeField] private float rotationSpeed = 10f; // Speed of rotation towards the movement direction
    [SerializeField] private GameObject pushObject;

    [Header("----------------")]
    public bool isGrabbing;
    public bool isCarrying;
    public bool canThrow;
    public bool canPush;
    public bool isGrounded;
    public bool isOnFreezer;

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
    private Rigidbody _rigidBody;

    [Header("Dash Properties")]
    [SerializeField] GameObject trailRenderer;
    [SerializeField] private float dashSpeed = 3f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 3f;
    public bool isDashing = false;
    private float dashTimer = 0f;
    private float cooldownTimer = 0f;
    private Vector3 dashDirection = Vector3.zero; // Direction of the dash

    [Header("Foot Trails")]
    public bool canPlayFootTrailParticle;
    public ParticleSystem footTrailParticleOne;
    public ParticleSystem footTrailParticleTwo;
    public ParticleSystem footTrailParticleThree;

    [Header("Player Audio")]
    public AudioManager _audioManager;

    void Start()
    {
        canPush = true;
        Invoke("Spawned", 2f);
        keyCodeOne = KeyCode.Space;

        // Script References
        _animator = GetComponent<Animator>();
        _playerJump = GetComponentInChildren<PlayerJump>();
        _playerGrab = GetComponentInChildren<PlayerGrab>();
        gamepadManager = FindObjectOfType<GamepadManager>();
        _rigidBody = GetComponent<Rigidbody>();
        _audioManager = FindObjectOfType<AudioManager>();

        // Dash Values
        dashSpeed = 30;
        dashDuration = 0.10f;
        dashCooldown = 3;

    }

    private void Update()
    {
        if (GameManager.instance.gameStart == true)
        {
            // Decide input method based on GamepadManager assignment
            bool useController = gamepadManager != null && gamepadManager.IsControllerAssignedToPlayer(playerID);
            Vector3 moveDirection = Vector3.zero;
            if (!isDashing)
            {
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
                    if (Input.GetButtonDown("Fire3_" + playerID) && cooldownTimer <= 0f) // "X" button
                    {
                        Debug.Log("Player " + playerID + " X button (controller) pressed");
                        AudioForDash();
                        isDashing = true;
                        dashTimer = dashDuration;
                        cooldownTimer = dashCooldown;
                        dashDirection = moveDirection != Vector3.zero ? moveDirection : transform.forward; // Save dash direction
                    }

                    // Trail Effect
                    if (moveDirection != Vector3.zero && canPlayFootTrailParticle == true && isGrounded == true)
                    {
                        FootTrailParticleOn();
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
                    if (Input.GetKeyDown(keyCodeFour) && cooldownTimer <= 0f)
                    {
                        Debug.Log(keyCodeFour + " : X button pressed");
                        AudioForDash();
                        isDashing = true;
                        dashTimer = dashDuration;
                        cooldownTimer = dashCooldown;
                        dashDirection = moveDirection != Vector3.zero ? moveDirection : transform.forward; // Save dash direction
                    }

                    // Trail Effect
                    if (moveDirection != Vector3.zero && canPlayFootTrailParticle == true && isGrounded == true)
                    {
                        FootTrailParticleOn();
                    }
                }
            }
            // Handle dashing
            if (isDashing)
            {
                if (dashTimer == dashDuration) // Apply the dash force only once
                {
                    _rigidBody.velocity = Vector3.zero; // Reset velocity before dashing
                    _rigidBody.AddForce(dashDirection * dashSpeed, ForceMode.Impulse); // Apply dash force
                }

                trailRenderer.SetActive(true);
                dashTimer -= Time.deltaTime;

                if (dashTimer <= 0f)
                {
                    isDashing = false;
                    trailRenderer.SetActive(false);
                    _rigidBody.velocity = Vector3.zero; // Stop any residual movement after dash
                }
            }
            else
            {
                cooldownTimer -= Time.deltaTime;
            }

            transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

            // ANIMATOR =================================
            UpdateAnimator(moveDirection);
            if (isCarrying == true && isOnFreezer == false)
            {
                speed = 5.5f;
            }
            if (isCarrying == false && isOnFreezer == false)
            {
                speed = 7f;
            }
            if (isCarrying == true && isOnFreezer == true)
            {
                speed = 5f;
            }
            if (isCarrying == false && isOnFreezer == true)
            {
                speed = 6.5f;
            }
        }

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

    public void Spawned()
    {
        this.gameObject.tag = "Player";
    }

    // UNITY EVENT SYSTEM METHODS
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
        AdjustItemPosition();
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
    public void AdjustItemPosition()
    {
        _playerGrab.AdjustIngredientPosition();
    }
    public void FootTrailParticleOn()
    {
        canPlayFootTrailParticle = false;
        footTrailParticleOne.Play();
        footTrailParticleTwo.Play();
        footTrailParticleThree.Play();
        Invoke("FootTrailParticleOff", 0.3f);
    }
    public void FootTrailParticleOff()
    {
        footTrailParticleOne.Stop();
        footTrailParticleTwo.Stop();
        footTrailParticleThree.Stop();
        canPlayFootTrailParticle = true;
    }

    public void AudioForWalk()
    {
        _audioManager.PlayWalkSound();
    }
    public void AudioForGrab()
    {
        _audioManager.PlayGrabSound();
    }

    public void AudioForThrow()
    {
        _audioManager.PlayThrowSound();
    }
    public void AudioForPush()
    {
        _audioManager.PlayPushSound();
    }
    public void AudioForDash()
    {
        _audioManager.PlayDashSound();
    }

}
