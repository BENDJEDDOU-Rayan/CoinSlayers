using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState state;
    public static event Action<GameState> OnGameStateChange;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject winScreen;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.Alive);
    }

    public void UpdateGameState(GameState newState)
    {
        state = newState;

        switch (newState)
        {
            case GameState.Paused:
                Debug.Log("Le jeu est en pause !");
                break;
            case GameState.Lose:
                HandleLose();
                break;
            case GameState.Win:
                HandleWin();
                break;
            case GameState.Alive:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = true;
                loseScreen.SetActive(false);
                break;
        }

        OnGameStateChange?.Invoke(newState);
    }

    private void HandleWin()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SoundManager.PlaySound(SoundType.GAME_WIN, 1f);
        winScreen.SetActive(true);
    }

    private void HandleLose()
    {
        Debug.Log("Le joueur est mort !");
        StartCoroutine(showLoseScreen(2));
    }

    private IEnumerator showLoseScreen(float delay)
    {
        yield return new WaitForSeconds(delay);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        loseScreen.SetActive(true);
    }

}

public enum GameState
{
    Lose,
    Win,
    Alive,
    Paused
}
