using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightAttack : MonoBehaviour
{

    [Header("Combat")]
    public float normalAttackRange = 5f;
    public float normalAttackDamage;
    public float normalAttackCooldown = 1f;
    public float normalKnockbackForce = 0f;
    public GameObject hitParticle;
    public PlayerControls playerControls;
    private Animator animator;
    public Transform orientation;
    private InputAction meleeAttackInputAction;
    private PlayerMovement movement;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        GameManager.OnGameStateChange += HandleGameStateUpdate;
    }

    private void OnEnable()
    {
        playerControls = new PlayerControls();
        meleeAttackInputAction = playerControls.Player.LightAttack;
        meleeAttackInputAction.performed += OnClickPerformed;
        meleeAttackInputAction.canceled += OnClickReleased;
        meleeAttackInputAction.Enable();
    }

    private void OnDisable()
    {
        meleeAttackInputAction.performed -= OnClickPerformed;
        meleeAttackInputAction.canceled -= OnClickReleased;
        meleeAttackInputAction.Disable();
    }

    private void HandleGameStateUpdate(GameState state)
    {
        if (state == GameState.Lose || state == GameState.Win)
        {
            meleeAttackInputAction.Disable();

        }
        if (state == GameState.Alive)
        {
            meleeAttackInputAction.Enable();
        }
    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        animator.SetTrigger("isPunching");
        animator.SetBool("Punching", true);
        movement.DisableMovement();
    }

    private void OnClickReleased(InputAction.CallbackContext context)
    {
        animator.SetBool("Punching", false);
        movement.EnableMovement();
    }

    public void HandlePunch()
    {
        Vector3 rayOrigin = orientation.position;
        rayOrigin.y += 1f;
        Vector3 rayDirection = orientation.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitInfo, normalAttackRange))
        {
            GameObject target = hitInfo.collider.gameObject;
            if (target == null){ return; }
            ZombieHealth zombie = target.GetComponent<ZombieHealth>();
            if (zombie == null || zombie.IsDead()){ return;}
            zombie.TakeDamage(normalAttackDamage);
            SoundManager.PlaySound(SoundType.MELEE_NORMAL, 0.5f);
            Instantiate(hitParticle, hitInfo.point, Quaternion.identity);
        }
        else
        {
            SoundManager.PlaySound(SoundType.MELEE_NORMAL_MISS, 1f);
        }

        Debug.DrawRay(rayOrigin, rayDirection * normalAttackRange, Color.red, 0.1f);
    }
}
