using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoardSpawner : MonoBehaviour 
{
    [SerializeField]
    private List<GameObject> _boardPrefabs; // List of board prefabs to spawn
    
    [SerializeField]
    private List<BoardStats> _boardStats; // List of board stats to spawn


    [SerializeField]
    private CandleController _candleController; // Reference to the CandleController script

    [SerializeField]
    private CrystalSlotController _crystalSlotController; // Reference to the CrystalSlotController script

    [SerializeField]
    private PlanchetteController _planchetteController; // Prefab for the planchette

    [SerializeField]
    private ThrowController _throwController; // Reference to the ThrowController script


    private BoardController _boardController; // Reference to the BoardController script
    private BoardStats boardStatsInstance;
    private PlanchetteController planchetteControllerInstance;

    private bool firstBoard = true;


    public void SpawnBoard()
    {
        //Get Random board
        int randomIndex = Random.Range(0, _boardPrefabs.Count);
        Debug.Log("Spawning board: " + randomIndex); // Log the index of the spawned board prefab
        _boardController = Instantiate(_boardPrefabs[randomIndex], transform).GetComponent<BoardController>(); // Instantiate the board prefab and get the BoardController component

        int randomStatsIndex = Random.Range(0, _boardStats.Count);
        if (firstBoard)
        {
            randomStatsIndex = 0;
            firstBoard = false; // Set firstBoard to false after the first spawn
        }

        //Get Random board stats
        Debug.Log("Using board stats: " + randomStatsIndex); // Log the index of the board stats used
        boardStatsInstance = Instantiate(_boardStats[randomStatsIndex]); // Get the random board stats
        planchetteControllerInstance = Instantiate(_planchetteController); // Instantiate the planchette controller prefab

        _boardController.PlanchetteController = planchetteControllerInstance; 
        _boardController.CandleController = _candleController; // Set the CandleController in the BoardController

        _crystalSlotController.SetBoardStats(boardStatsInstance); // Set the board stats in the CrystalSlotController
        _candleController.SetBoardStats(boardStatsInstance); // Set the board stats in the CandleController
        _throwController.SetBoardStats(boardStatsInstance); // Set the board stats in the ThrowController

        StartCoroutine(StartBoard()); // Start the board after a delay
    }

    IEnumerator StartBoard()
    {
        yield return new WaitForSeconds(2f); // Wait for 1 second before starting the board
        _boardController.StartBoard(); // Start the board
    }

    public void DestroyBoard()
    {
        if (_boardController != null)
        {
            Destroy(_boardController.gameObject); // Destroy the board game object
            _boardController = null; // Set the reference to null
        }
        if (boardStatsInstance != null)
        {
            Destroy(boardStatsInstance); // Destroy the board stats game object
            boardStatsInstance = null; // Set the reference to null
        }
        if (planchetteControllerInstance != null)
        {
            Destroy(planchetteControllerInstance.gameObject); // Destroy the planchette game object
            planchetteControllerInstance = null; // Set the reference to null
        }
    }
}
