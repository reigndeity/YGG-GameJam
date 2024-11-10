using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Properties")]
    [SerializeField] public float speed = 5f; // Speed of the player movement
    [SerializeField] private int playerID = 1; // Player ID (1, 2, 3, or 4)
    [SerializeField] private bool isKeyboard;  // Toggle to use keyboard or controller
    [SerializeField] private float rotationSpeed = 10f; // Speed of rotation towards the movement direction
    public bool canPush;

    [Header("Script References")]
    public GrabMechanics _grabMechanics;
    private Animator _animator;

    void Start()
    {
        _grabMechanics = GetComponentInChildren<GrabMechanics>();
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
            if (Input.GetButtonDown("Keyboard_A")) // "H" key
            {
                Debug.Log("Keyboard A button (H key) pressed");
                if (_grabMechanics.grabbedObject == null)
                {
                    _grabMechanics.GrabIngredient();
                }
                else
                {
                    _grabMechanics.Release();
                }                
            }

            if (Input.GetKeyDown(KeyCode.F) && canPush)
            {
                canPush = false;
                _animator.SetTrigger("Push");
            }

            if (Input.GetButtonDown("Keyboard_X")) // "J" key
            {
                Debug.Log("Keyboard X button (J key) pressed");
            }

            if (Input.GetButtonDown("Keyboard_B")) // "K" key
            {
                Debug.Log("Keyboard B button (K key) pressed");
            }

            if (Input.GetButtonDown("Keyboard_Y")) // "L" key
            {
                Debug.Log("Keyboard Y button (L key) pressed");
            }
        }
        else
        {
            // Joystick Movement (Controller)
            float horizontalInput = Input.GetAxis("Horizontal" + playerID);
            float verticalInput = Input.GetAxis("Vertical" + playerID);
            moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

            // Controller Button Inputs
            if (Input.GetButtonDown("Jump" + playerID)) // "X" button
            {
                Debug.Log("Player " + playerID + " A button (controller) pressed");
            }

            if (Input.GetButtonDown("Fire1_" + playerID)) // "A" button
            {
                Debug.Log("Player " + playerID + " X button (controller) pressed");
                if (_grabMechanics.grabbedObject == null)
                {
                    _grabMechanics.GrabIngredient();
                }
                else
                {
                    _grabMechanics.Release();
                }  
            }
            if (Input.GetButtonDown("Fire2_" + playerID)) // "B" button
            {
                Debug.Log("Player " + playerID + " B button (controller) pressed");
            }

            if (Input.GetButtonDown("Fire3_" + playerID)) // "Y" button
            {
                Debug.Log("Player " + playerID + " Y button (controller) pressed");
            }
        }

        // Move the player based on the selected input method
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        // Rotate the player to face the movement direction if there is movement
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            _animator.SetInteger("animState", 1);
        }
        else
        {
            _animator.SetInteger("animState", 0);
        }
    }
}
