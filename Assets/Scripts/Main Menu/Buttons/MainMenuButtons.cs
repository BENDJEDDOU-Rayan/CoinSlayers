using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private SceneController sceneController;
    [SerializeField] private FadeEffect fadeEffect;

    public void HandlePlay()
    {
        SoundManager.PlaySound(SoundType.UI_BTN_PRESSED, 1f);
        sceneController.LoadScene("Level");
    }

    public void HandleExit()
    {
        SoundManager.PlaySound(SoundType.UI_BTN_PRESSED, 1f);
        StartCoroutine(fadeEffect.FadeOutCoroutine(1f));
        Application.Quit();
    }

    public void HandleOpenSettings()
    {
        SoundManager.PlaySound(SoundType.UI_BTN_PRESSED, 1f);
    }

    public void HandleCreditsOpen()
    {
        SoundManager.PlaySound(SoundType.UI_BTN_PRESSED, 1f);
    }
}
