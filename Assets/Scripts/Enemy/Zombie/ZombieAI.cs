using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    private bool playerDetected = false;
    private GameObject player;
    private ZombieAttack zombieAttack;
    private ZombieMovement movement;
    private ZombieHealth health;
    private PlayerHealth playerHealth;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        zombieAttack = GetComponent<ZombieAttack>();
        movement = GetComponent<ZombieMovement>();
        health = GetComponent<ZombieHealth>();
        if (player == null)
        {
            Debug.LogWarning("Le player est introuvable !");
        }
        if (playerHealth == null)
        {
            Debug.LogWarning("playerHealth est null !");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerDetected = true;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player) { 
            playerDetected = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            StartCoroutine(ExitChase(5));
        }
    }

    private IEnumerator ExitChase(float duration)
    {
        yield return new WaitForSeconds(duration);
        playerDetected = false;
    }

    private void Update()
    {
        if (PlayerIsInFront() && !playerHealth.isDead)
        {
            zombieAttack.HandleAttack();
        }
        if (playerDetected && !zombieAttack.isAttacking && !health.isStunned && !health.isDead && !playerHealth.isDead)
        {
            movement.MoveToPlayer(player);
        }
    }

    private bool PlayerIsInFront()
    {
        Ray ray = new(transform.position + Vector3.up * 1f, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1f))
        {
            Debug.DrawRay(ray.origin, ray.direction * 1f, Color.green, 0.1f);
            return hitInfo.collider.gameObject == player;
        }

        Debug.DrawRay(ray.origin, ray.direction * 1f, Color.red, 0.1f);
        return false;
    }
}
