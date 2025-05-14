using Unity.VisualScripting;
using UnityEngine;

public class Slot : MonoBehaviour
{

    public bool isOccupied; // Whether the slot is occupied or not
    public CrystalType crystalType;
    public bool isCorrect = false; // Whether the slot is correct or not

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component from the slot's transform
    }


    public void SetColorSuccess()
    {
        spriteRenderer.color = Color.green; // Set the color to green
        isCorrect = true; // Mark the slot as correct
    }

    public void SetCollerWrong()
    {
        spriteRenderer.color = Color.red; // Set the color to red
        isCorrect = false; // Mark the slot as incorrect
    }

    public void SetColorDefault()
    {
        spriteRenderer.color = Color.white; // Set the color to white
        isCorrect = false; // Mark the slot as incorrect
    }
}
