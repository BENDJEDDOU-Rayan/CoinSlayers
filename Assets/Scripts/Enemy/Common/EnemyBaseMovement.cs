using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    protected Transform player;
    public float stopDistance = 1f;

    protected virtual void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    protected virtual void Update()
    {
        if (player != null)
        {
            FollowPlayer();
        }
    }

    protected virtual void FollowPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > stopDistance)
        {
            transform.Translate(directionToPlayer * moveSpeed * Time.deltaTime, Space.World);
        }
        RotateTowardsPlayer(directionToPlayer);
    }

    protected virtual void RotateTowardsPlayer(Vector3 directionToPlayer)
    {
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);
    }
}
