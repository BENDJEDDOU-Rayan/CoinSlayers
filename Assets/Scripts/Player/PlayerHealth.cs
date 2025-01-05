using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public bool isDead = false;
    [SerializeField] private float health = 100f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private GameObject respawnEffect;
    private Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float amount)
    {
        if (isDead)
            return;
        SoundManager.PlaySound(SoundType.PLAYER_PAIN, 0.5f);
        health -= amount;
        if(health <= 0)
        {
            Die();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            transform.position = new Vector3(0, 12, 0);
        }
    }

    public void Die()
    {
        SoundManager.PlaySound(SoundType.PLAYER_DEATH, 0.5f);
        health = 0;
        isDead = true;
        animator.SetBool("isDead", true);
        GameManager.Instance.UpdateGameState(GameState.Lose);
    }

    public void Respawn()
    {
        SoundManager.PlaySound(SoundType.PLAYER_RESPAWN, 0.5f);
        animator.SetBool("isDead", false);
        isDead = false;
        health = 100f;
        GameManager.Instance.UpdateGameState(GameState.Alive);
        Instantiate(respawnEffect, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z), transform.rotation);
        transform.position = new Vector3(5, 1, -9);
        Debug.Log("Le joueur vient de réapparaître !");
    }

    public float GetCurrentHealth()
    {
        return health;
    }

    public void Heal(float amount)
    {
        health += amount;
    }

    public void HealMax()
    {
        health = maxHealth;
    }
}
