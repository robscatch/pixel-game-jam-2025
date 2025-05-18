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

    public PlanchetteController PlanchetteController { get => _planchetteController; set => _planchetteController = value; }
    public CandleController CandleController { get => _candleController; set => _candleController = value; }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var letterObjects = GetComponentInChildren<Transform>();
        foreach (Transform letterObject in letterObjects)
        {
            _letterPositions.Add(letterObject.name, letterObject);
            letterObject.GetComponent<SpriteRenderer>().enabled = false; // Disable the sprite renderer for each letter object
        }

    }

    public void StartBoard()
    {
        PlanchetteController.StartAutoMovement += MovePlanchette;
        MovePlanchette();
    }

    private void MovePlanchette()
    {
        // Randomly move the planchette to a letter position
        int randomIndex = Random.Range(0, _letterPositions.Count);
        string randomLetter = new List<string>(_letterPositions.Keys)[randomIndex];
        Transform targetPosition = _letterPositions[randomLetter];

        PlanchetteController.SetNewPosition(targetPosition.position);
    }
}
