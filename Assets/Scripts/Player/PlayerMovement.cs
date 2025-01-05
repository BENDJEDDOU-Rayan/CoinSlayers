using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed;
    public float groundDrag;
    public float sprintSpeedMultiplier;
    private bool isSprinting;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;
    Vector3 moveDirection;
    private static Rigidbody rb;
    private Animator anim;
    private PlayerControls inputActions;

    private void Awake()
    {
        inputActions = new PlayerControls();
        GameManager.OnGameStateChange += HandleGameStateUpdate;
    }

    private void HandleGameStateUpdate(GameState state)
    {
        if(state == GameState.Lose || state == GameState.Win)
        {
            inputActions.Player.Disable();

        }
        if (state == GameState.Alive)
        {
            inputActions.Player.Enable();
        }
    }

    private void OnEnable()
    {
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Sprint.performed += StartSprinting;
        inputActions.Player.Sprint.canceled += StopSprinting;
        inputActions.Player.Move.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Sprint.performed -= StartSprinting;
        inputActions.Player.Sprint.canceled -= StopSprinting;
        inputActions.Player.Move.Disable();
    }

    public void DisableMovement()
    {
        inputActions.Player.Move.Disable();
    }

    public void EnableMovement()
    {
        inputActions.Player.Move.Enable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        SpeedControl();
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        anim.SetFloat("moveSpeed", horizontalVelocity.magnitude);
        if (grounded)
        {
            if (anim.GetBool("isJumping"))
            {
                anim.SetBool("isJumping", false);
            }
            rb.drag = groundDrag;
        }
        else
        {
            if (!anim.GetBool("isJumping"))
            {
                anim.SetBool("isJumping", true);
            }
            anim.SetBool("isJumping", true);
            rb.drag = 0;
        }
            
    }

    public static Rigidbody GetRb()
    {
        return rb;
    }

    public static void SetRigidbody(Rigidbody newRb)
    {
        rb = newRb;
    }

    private void FixedUpdate()
    {
        HandleMove();
    }

    private void HandleMove()
    {
        if (HeavyAttack.IsHeavyAttacking)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            Debug.Log("Tu ne peux plus bouger normalement !");
            return;
        }
        moveDirection = orientation.forward * inputActions.Player.Move.ReadValue<Vector2>().y + orientation.right * inputActions.Player.Move.ReadValue<Vector2>().x;
        float speed = isSprinting ? walkSpeed * sprintSpeedMultiplier : walkSpeed;
        if (grounded)
        {
            rb.AddForce(10f * speed * moveDirection.normalized, ForceMode.Force);
            
        }
        else
        {
            rb.AddForce(10f * airMultiplier * speed * moveDirection.normalized, ForceMode.Force);
            
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new(rb.velocity.x, 0f, rb.velocity.z);
        float speedLimit = isSprinting ? walkSpeed * sprintSpeedMultiplier : walkSpeed;
        if (flatVel.magnitude > speedLimit)
        {
            Vector3 limitedVel = flatVel.normalized * speedLimit;
            rb.velocity = new(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (HeavyAttack.IsHeavyAttacking) return;
        if (readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
            anim.SetTrigger("Jump");
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void StartSprinting(InputAction.CallbackContext context)
    {
        isSprinting = true;
        anim.SetBool("isSprinting", true);
    }

    private void StopSprinting(InputAction.CallbackContext context)
    {
        isSprinting = false;
        anim.SetBool("isSprinting", false);
    }
}
