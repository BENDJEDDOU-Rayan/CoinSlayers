using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    private Animator animator;
    public bool isAttacking;
    private bool canAttack = true;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void HandleAttack()
    {
        if (canAttack)
        {
            isAttacking = true;
            int random = Random.Range(1, 3);
            animator.SetTrigger("attack" + random);
            StartCoroutine(AttackCooldown());
            // ThrowAttackRaycast();
        }
    }

    public void ThrowAttackRaycast()
    {
        Ray ray = new(transform.position + Vector3.up * 1f, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 2f))
        {
            if (hitInfo.collider.CompareTag("Player"))
            {
                PlayerHealth playerHealth = hitInfo.collider.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth != null) {
                    playerHealth.TakeDamage(5);
                    SoundManager.PlaySound(SoundType.MELEE_NORMAL);
                } else
                {
                    SoundManager.PlaySound(SoundType.MELEE_NORMAL_MISS);
                }
            }
        }
        Debug.DrawRay(ray.origin, ray.direction * 2f, Color.red, 0.1f);
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(2);
        canAttack = true;
        isAttacking = false;
    }
}
