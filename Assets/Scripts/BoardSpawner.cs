using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSpawner : Manager<BoardSpawner>
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


    private BoardController _boardController; // Reference to the BoardController script
    private BoardStats boardStatsInstance;



    public void SpawnBoard()
    {
        //Get Random board
        int randomIndex = Random.Range(0, _boardPrefabs.Count);
        _boardController = Instantiate(_boardPrefabs[randomIndex], transform).GetComponent<BoardController>(); // Instantiate the board prefab and get the BoardController component

        //Get Random board stats
        int randomStatsIndex = Random.Range(0, _boardStats.Count);
        boardStatsInstance = Instantiate(_boardStats[randomStatsIndex]); // Get the random board stats
        
        _boardController.PlanchetteController = _planchetteController; 
        _boardController.CandleController = _candleController; // Set the CandleController in the BoardController

        _crystalSlotController.SetBoardStats(boardStatsInstance); // Set the board stats in the CrystalSlotController
        _candleController.SetBoardStats(boardStatsInstance); // Set the board stats in the CandleController

        StartCoroutine(StartBoard()); // Start the board after a delay
    }

    IEnumerator StartBoard()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second before starting the board
        _boardController.StartBoard(); // Start the board
    }
}
