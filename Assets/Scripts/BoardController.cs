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
    private BoardStats _boardStats;

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


        _planchetteController.OnDragging += MovePlanchette;

        // Check if the board is haunted
        IsHaunted = _boardStats.DoesBoardControlPlanchette || _boardStats.DoesBoardControlCandle;

    }


    private void MovePlanchette()
    {
        Debug.Log("Planchette is being dragged!");
        Debug.Log("boardis haunted: " + _boardStats.DoesBoardControlPlanchette);
        if (_boardStats.DoesBoardControlPlanchette)
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
