using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameManager : Manager<GameManager>
{
    public bool IsGamePaused { get; internal set; }
    public int NumBoardsCleared { get => numBoardsCleared; set => numBoardsCleared = value; }

    public AudioClip TitleTheme;

    public AudioClip playerDeadTheme;
    public AudioClip MainTheme;

    [SerializeField]
    CountDownTimer ShiftTimer;

    [SerializeField]
    private BoardSpawner _boardSpawner;

    public bool playerIsDead = false; // Flag to check if the player is dead

    private int numBoardsCleared = 0; // Number of boards cleared by the player

    private bool firstTime = true;


    [SerializeField]
    private GameObject vignettePrefab;

    private GameObject vignette; // Reference to the vignette effect
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundManager.Instance.PlayLoop(TitleTheme); // Play the title theme music
        ShiftTimer.Finished += PlayerWins; // Subscribe to the ShiftTimer finished event
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

    public void IncrementNumBoardsCleared()
    {
        numBoardsCleared++; // Increment the number of boards cleared
        Debug.Log($"Num Boards Cleared: {numBoardsCleared}"); // Log the number of boards cleared
    }
    
    public void DestroyBoard()
    {
        _boardSpawner.DestroyBoard(); // Destroy the game board
    }


    public void SpawnBoard()
    {
        SoundManager.Instance.PlayTheme(MainTheme);
        _boardSpawner.SpawnBoard(); // Spawn the game board
    }


    public void PlayerDied()
    {
        Debug.Log("Game Over"); // Log the game over action
        UIManager.Instance.GameOver($"You have died!\n You clensed {NumBoardsCleared} boards."); // Show the game over UI
        SoundManager.Instance.PlayLoop(playerDeadTheme);
        Cleanup();
    }

    private void Cleanup()
    {
        Time.timeScale = 0; // Stop the game time
        playerIsDead = true; // Set the player dead flag to true
        if (vignette != null)
        {
            Destroy(vignette.gameObject); // Destroy the vignette effect if it exists
        }
        ShiftTimer.Stop(); // Stop the shift timer
        DestroyBoard(); // Clear the game board
    }


    public void PlayerWins()
    {
        Debug.Log("Player Wins"); // Log the player win action
        UIManager.Instance.GameOver($"You survived your shift!\n You clensed {NumBoardsCleared} boards."); // Show the game over UI
        Cleanup();
    }

    public void InitGame()
    {
        if (firstTime)
        {
            UIManager.Instance.DisplayIntroPanel(); // Show the intro panel
            firstTime = false; // Set first time to false
            StartCoroutine(DelayInit()); // Start the game initialization after a delay
            return;
        }

        playerIsDead = false; // Reset the player dead flag
        SoundManager.Instance.PlayTheme(MainTheme); // Play the main theme music
        Debug.Log("Game Initialized"); // Log the game initialization
        Time.timeScale = 1; // Resume the game time
        numBoardsCleared = 0; // Reset the number of boards cleared

        ShiftTimer.Begin(); // Start the shift timer
        _boardSpawner.SpawnBoard(); // Spawn the game board
    }

    IEnumerator DelayInit()
    {
        yield return new WaitForSeconds(7f); // Wait for 1 second
        UIManager.Instance.HideIntroPanel(); // Hide the intro panel
        InitGame(); // Initialize the game
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

    internal void StartWarning()
    {
        vignette = Instantiate(vignettePrefab); // Instantiate the vignette effect
    }
}
