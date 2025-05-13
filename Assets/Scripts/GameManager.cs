using System;
using UnityEngine;

public class GameManager : Manager<GameManager>
{
    public bool IsGamePaused { get; internal set; }

    public AudioClip TitleTheme;
    public AudioClip mainTheme;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundManager.Instance.PlayTheme(TitleTheme); // Play the title theme music
        Time.timeScale = 0; // Stop the game time

    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public void InitGame()
    {
        SoundManager.Instance.PlayTheme(mainTheme); // Play the main theme music
        Debug.Log("Game Initialized"); // Log the game initialization
        Time.timeScale = 1; // Resume the game time
    }

    internal void ResumeGame()
    {
        IsGamePaused = false; // Set the game state to running
        Time.timeScale = 1; // Resume the game time
    }

    internal void PauseGame()
    {
        // Pause the game logic here
        IsGamePaused = true; // Set the game state to paused
        Time.timeScale = 0; // Stop the game time
    }

    internal void QuitGame()
    {
        Debug.Log("Quit Game"); // Log the quit action
        Application.Quit(); // Quit the application
    }
}
