using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Manager<GameManager>
{
    public bool IsGamePaused { get; internal set; }

    public AudioClip TitleTheme;
    public AudioClip mainTheme;

    public GameObject[] SystemPrefabs; // Array of system prefabs to be instantiated

    private List<GameObject> _instancedSystemPrefabs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        _instancedSystemPrefabs = new List<GameObject>();
        InstantiateSystemPrefabs();

        SoundManager.Instance.PlayTheme(TitleTheme); // Play the title theme music

    }

    private void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance;
        for (int i = 0; i < SystemPrefabs.Length; ++i)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);
            _instancedSystemPrefabs.Add(prefabInstance);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public void InitGame()
    {
        SoundManager.Instance.PlayTheme(mainTheme); // Play the main theme music
        Debug.Log("Game Initialized"); // Log the game initialization
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
