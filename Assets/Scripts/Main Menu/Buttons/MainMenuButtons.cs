using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private SceneController sceneController;

    public void HandlePlay()
    {
        SoundManager.PlaySound(SoundType.UI_BTN_PRESSED, 1f);
        sceneController.LoadScene("Level");
    }

    public void HandleExit()
    {
        SoundManager.PlaySound(SoundType.UI_BTN_PRESSED, 1f);
        Application.Quit();
    }

    public void HandleOpenSettings()
    {

    }
}
