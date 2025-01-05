using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseMenuButtons : MonoBehaviour
{
    private PlayerHealth playerHealth;
    [SerializeField] private SceneController sceneController;

    private void Awake()
    {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
    }

    public void HandleRespawn()
    {
        SoundManager.PlaySound(SoundType.UI_BTN_PRESSED, 1f);
        playerHealth.Respawn();
    }

    public void HandleQuit()
    {
        SoundManager.PlaySound(SoundType.UI_BTN_PRESSED, 1f);
        sceneController.LoadScene("MainMenu");
    }
}
