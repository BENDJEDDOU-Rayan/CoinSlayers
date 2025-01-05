using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ZombieHealth : MonoBehaviour, IStunnable
{

    public Healthbar healthBar;
    public bool isStunned = false;
    public GameObject damageText;
    public GameObject stunParticles;
    private float health = 100f;
    private float maxHealth = 100f;
    private CapsuleCollider hitBox;
    private Animator anim;
    public bool isDead = false;
    [SerializeField] private GameObject healthBarCanvas;

    public event Action OnDeath;
    public event Action OnStun;
    public event Action OnUnstun;

    private void Start()
    {
        anim = GetComponent<Animator>();
        hitBox = GetComponentInChildren<CapsuleCollider>();
    }

    public void TakeDamage(float amount)
    {
        if (isDead)
            return;
        health -= amount;
        SoundManager.PlaySound(SoundType.ZOMBIE_PAIN, 1f);
        healthBar.UpdateHealth(health / maxHealth);
        DamageIndicator damageIndicator = Instantiate(damageText, transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
        damageIndicator.SetDamageText(amount);
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    public void Die()
    {
        anim.SetBool("isStunned", false);
        anim.SetBool("isDead", true);
        isDead = true;
        hitBox.enabled = false;
        healthBarCanvas.SetActive(false);
        SoundManager.PlaySound(SoundType.ZOMBIE_DEAD, 1f);
        StartCoroutine(ZombieCleanup());
        OnDeath?.Invoke();
    }

    private IEnumerator ZombieCleanup()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    public void Stun(float duration)
    {
        OnStun?.Invoke();
        isStunned = true;
        Instantiate(stunParticles, new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z), Quaternion.identity).transform.SetParent(this.transform);
        SoundManager.PlaySound(SoundType.STUNNED, 1f);
        anim.SetBool("isStunned", true);
        StartCoroutine(UnstunCoroutine(duration));
    }

    public bool IsDead()
    {
        return isDead;
    }

    private IEnumerator UnstunCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        Unstun();
    }

    public void Unstun()
    {
        OnUnstun?.Invoke();
        isStunned = false;
        anim.SetBool("isStunned", false);
    }
}
