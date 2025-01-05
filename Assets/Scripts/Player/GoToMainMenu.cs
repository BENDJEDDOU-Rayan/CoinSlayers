using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMainMenu : MonoBehaviour
{
    [SerializeField] private SceneController sceneController;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            sceneController.LoadScene("MainMenu");
        }
    }
}
