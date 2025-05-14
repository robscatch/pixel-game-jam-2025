using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChantController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private CrystalSlotController _crystalSlotController; // Reference to the CrystalSlotController script
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_crystalSlotController.AllCorrect())
        {
            Debug.Log("All crystals are correct!"); // Log message when all crystals are correct
            SoundManager.Instance.PlaySingle(null);
            BoardSpawner.Instance.DestroyBoard(); // Clear the game board
            StartCoroutine(WaitBeforeSpawning()); // Wait before spawning a new board
        }
        else
        {
            Debug.Log("Not all crystals are correct!"); // Log message when not all crystals are correct
            SoundManager.Instance.PlaySingle(null);
        }
    }

    IEnumerator WaitBeforeSpawning() 
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second
        BoardSpawner.Instance.SpawnBoard(); // Spawn the game board again
    }
}
