using Unity.VisualScripting;
using UnityEngine;

public class Slot : MonoBehaviour
{

    public bool isOccupied; // Whether the slot is occupied or not
    public CrystalType crystalType;

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component from the slot's transform
    }


    public void SetColorSuccess()
    {
        spriteRenderer.color = Color.green; // Set the color to green

    }

    public void SetCollerWrong()
    {
        spriteRenderer.color = Color.red; // Set the color to red
    }

    public void SetColorDefault()
    {
        spriteRenderer.color = Color.white; // Set the color to white
    }
}
