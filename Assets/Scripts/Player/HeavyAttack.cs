using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class HeavyAttack : MonoBehaviour
{
    public static bool IsHeavyAttacking { get; private set; } = false;

    [Header("Combat")]
    public float heavyAttackRange = 5f;
    public float heavyAttackDamage;
    public float heavyAttackCooldown = 1f;
    public float heavyAttackKnockbackForce = 5f;

    [Header("Detection")]
    public Transform orientation;
    public GameObject hitParticle;

    private Animator anim;
    private PlayerControls playerControls;
    private InputAction heavyAttackInputAction;
    private PlayerMovement movement;

    void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
    }
    void Awake()
    {
        playerControls = new PlayerControls();
        heavyAttackInputAction = playerControls.Player.HeavyAttack;
        heavyAttackInputAction.performed += ctx => OnHeavyAttackPerformed();
        GameManager.OnGameStateChange += HandleGameStateUpdate;
    }

    private void OnEnable()
    {
        heavyAttackInputAction.Enable();
    }
    private void OnDisable()
    {
        heavyAttackInputAction.Disable();
    }

    private void HandleGameStateUpdate(GameState state)
    {
        if (state == GameState.Lose)
        {
            heavyAttackInputAction.Disable();

        }
        if (state == GameState.Alive)
        {
            heavyAttackInputAction.Enable();
        }
    }

    private void OnHeavyAttackPerformed()
    {
        if (!IsHeavyAttacking)
        {
            PerformHeavyAttack();
        }
    }
    public void PerformHeavyAttack()
    {
        IsHeavyAttacking = true;
        anim.SetTrigger("HeavyAttack");
        heavyAttackInputAction.Disable();
        StartCoroutine(EndHeavyAttack());
    }

    public void KickRaycast()
    {
        Vector3 rayOrigin = orientation.position;
        rayOrigin.y += 1f;
        Vector3 rayDirection = orientation.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitInfo, heavyAttackRange))
        {
            HandleHit(hitInfo, rayDirection);
        }
        else
        {
            SoundManager.PlaySound(SoundType.MELEE_MISS, 1f);
        }

        Debug.DrawRay(rayOrigin, rayDirection * heavyAttackRange, Color.red, 0.1f);
    }

    public void HandleHit(RaycastHit hitInfo, Vector3 rayDirection)
    {
        Instantiate(hitParticle, hitInfo.point, Quaternion.identity);

        ZombieHealth zombie = hitInfo.collider.GetComponentInParent<ZombieHealth>();
        if (zombie != null)
        {
            IStunnable stunnable = zombie.GetComponent<IStunnable>();
            stunnable?.Stun(1f);
            zombie.TakeDamage(heavyAttackDamage);
            SoundManager.PlaySound(SoundType.MELEE_HEAVY, 0.3f);
        }

        Rigidbody targetRb = hitInfo.rigidbody;
        if (targetRb != null)
        {
            Vector3 knockbackDirection = rayDirection.normalized;
            targetRb.AddForce(knockbackDirection * heavyAttackKnockbackForce, ForceMode.Impulse);
        }
    }


    private IEnumerator EndHeavyAttack()
    {
        // Wait until the "HeavyMeleeAttack" animation is completely finished
        while (anim.GetCurrentAnimatorStateInfo(0).IsName("HeavyMeleeAttack") &&
           anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            // Affiche l'état de l'animation pour le débogage
            Debug.Log("Tu es dans l'animation HeavyMeleeAttack avec normalizedTime: " +
                      anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
            yield return null; // Attendre un frame
        }
        // Set IsHeavyAttacking to false after the animation finishes
        IsHeavyAttacking = false;

        // Now wait for the cooldown
        yield return new WaitForSeconds(heavyAttackCooldown);

        // After the cooldown, re-enable actions
        heavyAttackInputAction.Enable();
    }


}
