using UnityEngine;
using UnityEngine.InputSystem;

public class CamMovement : MonoBehaviour
{
    public float intensity = 0.1f;
    public float smoothSpeed = 5f;
    public PlayerControls playerControls;
    private InputAction lookAction;
    private Vector3 initialPosition;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        lookAction = playerControls.Player.Look;
        lookAction.Enable();
    }

    private void OnDisable()
    {
        lookAction.Disable();
    }

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        Vector3 targetPosition = initialPosition + new Vector3(lookAction.ReadValue<Vector2>().x * intensity, lookAction.ReadValue<Vector2>().y * intensity, 0);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
