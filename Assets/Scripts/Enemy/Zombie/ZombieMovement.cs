using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Animator animator;
    private Rigidbody rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float speed = rb.velocity.magnitude;
        animator.SetFloat("speed", speed);
    }

    public void MoveToPlayer(GameObject player)
    {
        Vector3 playerPosition = new(player.transform.position.x, transform.position.y, player.transform.position.z);
        Vector3 directionToPlayer = (playerPosition - transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f));

        rb.velocity = directionToPlayer * moveSpeed;
    }
}
