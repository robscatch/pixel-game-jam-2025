using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoardController : MonoBehaviour
{
    Dictionary<string, Transform> _letterPositions = new Dictionary<string, Transform>();

    [SerializeField]
    private PlanchetteController _planchetteController;

    [SerializeField]
    private CandleController _candleController;


    [SerializeField]
    private BoardStats _boardStats_SO;
    private BoardStats BoardStatsInstance;

    [SerializeField]
    private bool IsHaunted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var letterObjects = GetComponentInChildren<Transform>();
        foreach (Transform letterObject in letterObjects)
        {
            Debug.Log(letterObject.name);
            _letterPositions.Add(letterObject.name, letterObject);
        }

        BoardStatsInstance = Instantiate(_boardStats_SO);

        _planchetteController.OnDragging += MovePlanchette;

        // Check if the board is haunted
        IsHaunted = BoardStatsInstance.DoesBoardControlPlanchette || BoardStatsInstance.DoesBoardControlCandle;

    }


    private void MovePlanchette()
    {
        Debug.Log("Planchette is being dragged!");
        Debug.Log("boardis haunted: " + BoardStatsInstance.DoesBoardControlPlanchette);
        if (BoardStatsInstance.DoesBoardControlPlanchette)
        {
            Debug.Log("Board is haunted! Moving planchette randomly.");
            // Randomly move the planchette to a letter position
            int randomIndex = Random.Range(0, _letterPositions.Count);
            string randomLetter = new List<string>(_letterPositions.Keys)[randomIndex];
            Transform targetPosition = _letterPositions[randomLetter];
            
            _planchetteController.SetNewPosition(targetPosition.position);
        }
    }

}
