using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerModel;
    public Rigidbody rb;
    public float rotationSpeed;
    public PlayerControls playerControls;
    private InputAction moveAction;

    private void Awake()
    {
        playerControls = new PlayerControls();
        GameManager.OnGameStateChange += HandleGameStateUpdate;
    }

    private void OnEnable()
    {
        if (moveAction == null)
        {
            moveAction = playerControls.Player.Move;
        }
        if (!moveAction.enabled)
        {
            moveAction.Enable();
        }
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }

    private void HandleGameStateUpdate(GameState state)
    {
        if (state == GameState.Lose)
        {
            moveAction.Disable();

        }
        if (state == GameState.Alive)
        {
            moveAction.Enable();
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!moveAction.enabled) return;
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;
        Vector3 inputDir = orientation.forward * moveAction.ReadValue<Vector2>().y + orientation.right * moveAction.ReadValue<Vector2>().x;
        if(inputDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDir.normalized);
            playerModel.rotation = Quaternion.Slerp(playerModel.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
            
    }
}
