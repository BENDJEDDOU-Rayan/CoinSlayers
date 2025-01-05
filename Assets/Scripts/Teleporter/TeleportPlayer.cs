using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SoundManager.PlaySound(SoundType.TELEPORTER_USE, 1f);
            other.transform.position = new Vector3(0, 12, 0);
        }
    }
}
