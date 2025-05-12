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
    private bool IsBoardHaunted = false;

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

        IsBoardHaunted = Random.Range(0f, 1f) < 0.5f;
    }


    private void MovePlanchette()
    {
        Debug.Log("Planchette is being dragged!");
        Debug.Log("boardis haunted: " + IsBoardHaunted);
        if (IsBoardHaunted)
        {
            Debug.Log("Board is haunted! Moving planchette randomly.");
            // Randomly move the planchette to a letter position
            int randomIndex = Random.Range(0, _letterPositions.Count);
            string randomLetter = new List<string>(_letterPositions.Keys)[randomIndex];
            Transform targetPosition = _letterPositions[randomLetter];
            
            _planchetteController.DisableDragging(); // Disable dragging when the board is controlling the planchette
            _planchetteController.transform.position = targetPosition.position;
        }
    }

}
