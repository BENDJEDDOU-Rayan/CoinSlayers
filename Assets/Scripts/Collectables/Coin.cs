using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private SphereCollider sCollider;
    void Start()
    {
        SphereCollider sCollider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SoundManager.PlaySound(SoundType.PLAYER_PICKUP_COIN, 1f);
            ScoreManager.scoreCount += 1;
            Destroy(gameObject);
        }
    }
}
