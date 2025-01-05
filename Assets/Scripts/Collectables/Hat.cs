using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : MonoBehaviour
{
    [SerializeField] private GameObject hat;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.PlaySound(SoundType.PICKUP_ITEMS, 1f);
            hat.SetActive(true);
            Destroy(gameObject);
        }
    }
}
