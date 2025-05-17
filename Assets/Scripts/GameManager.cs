using System;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : Manager<GameManager>
{
    public bool IsGamePaused { get; internal set; }
    public int NumBoardsCleared { get => numBoardsCleared; set => numBoardsCleared = value; }

    public AudioClip TitleTheme;

    public AudioClip playerDeadTheme;
    public AudioClip MainTheme;

    private int currentThemeIndex = 0;

    [SerializeField]
    CountDownTimer deathTimer;

    [SerializeField]
    CountDownTimer ShiftTimer;

    [SerializeField]
    private BoardSpawner _boardSpawner;

    public bool playerIsDead = false; // Flag to check if the player is dead

    private int numBoardsCleared = 0; // Number of boards cleared by the player


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundManager.Instance.PlayTheme(TitleTheme); // Play the title theme music
        Time.timeScale = 0; // Stop the game time
        deathTimer.Finished += PlayerDied;
        ShiftTimer.Finished += PlayerWins; // Subscribe to the ShiftTimer finished event
        ShiftTimer.Begin(); // Start the shift timer
    }

    public int GetTimeRemaining()
    {
        return ShiftTimer.timeRemaining; // Return the remaining time of the shift timer
    }

    public int GetShiftDuration()
    {
        return ShiftTimer.duration; // Return the duration of the shift timer
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public void DestroyBoard()
    {
        _boardSpawner.DestroyBoard(); // Destroy the game board
        NumBoardsCleared++;
    }


    public void SpawnBoard()
    {
        SoundManager.Instance.PlayTheme(MainTheme);
        _boardSpawner.SpawnBoard(); // Spawn the game board
    }


    public void PlayerPendingDeath()
    {
        Debug.Log("Player is pending death"); // Log the player death action
        deathTimer.Begin();
    }

    public void PlayerDied()
    {
        Debug.Log("Game Over"); // Log the game over action
        SoundManager.Instance.PlayTheme(playerDeadTheme);
        GameManager.Instance.DestroyBoard(); // Clear the game board
        playerIsDead = true; // Set the player dead flag to true
        Time.timeScale = 0; // Stop the game time
        UIManager.Instance.GameOver($"You have died!\n You clensed {NumBoardsCleared} boards."); // Show the game over UI
        StopAllTimers();
    }

    private void StopAllTimers()
    {
        deathTimer.Stop(); // Stop the death timer
        ShiftTimer.Stop(); // Stop the shift timer
    }

    public void PlayerWins()
    {
        Debug.Log("Player Wins"); // Log the player win action
        GameManager.Instance.DestroyBoard(); // Clear the game board
        playerIsDead = true;
        Time.timeScale = 0; // Stop the game time
        UIManager.Instance.GameOver($"You survived your shift!\n You clensed {NumBoardsCleared} boards."); // Show the game over UI
        StopAllTimers();
    }

    public void InitGame()
    {
        playerIsDead = false; // Reset the player dead flag
        SoundManager.Instance.PlayTheme(MainTheme); // Play the main theme music
        Debug.Log("Game Initialized"); // Log the game initialization
        Time.timeScale = 1; // Resume the game time

        _boardSpawner.SpawnBoard(); // Spawn the game board
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
