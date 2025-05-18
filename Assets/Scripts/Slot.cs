using UnityEngine;

public class Slot : MonoBehaviour
{

    public bool isOccupied; // Whether the slot is occupied or not
    public CrystalType crystalType;
    public bool isCorrect = false; // Whether the slot is correct or not

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    public CandleController candleController; // Reference to the CandleController script

    [SerializeField]
    private float snapRange = 0.5f; // Distance within which to snap

    private CrystalType lastCrystal;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component from the slot's transform
    }

    private void Update()
    {
        if (!candleController.IsFlameOn && isOccupied)
        {
            SetColorDefault();
        }
        else if (candleController.IsFlameOn && isOccupied)
        {
            VerifyCrystals(lastCrystal); // Verify the crystal type when the candle is on
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<CrystalController>() == null)
        {
            return;
        }

        var draggable = collision.gameObject.GetComponent<Draggable>();

        if (!draggable.IsDragging && !isOccupied)
        {
            var crystal = collision.gameObject.transform.GetComponent<CrystalController>().CrystalType;
            lastCrystal = crystal; // Store the last crystal type
            if (candleController.IsFlameOn)
            {
                VerifyCrystals(crystal);
            }

            isOccupied = true;
            collision.gameObject.transform.position = transform.position; // Snap to the position
            collision.gameObject.transform.rotation = transform.rotation; // Snap to the rotation
        }
    }


    private void VerifyCrystals(CrystalType crystal)
    {
        //Check crystal type
        if (crystalType != crystal)
        {
            SetCollerWrong(); // Set the color to red
        }
        else
        {
            SetColorSuccess(); // Set the color to green
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<CrystalController>() == null)
        {
            return;
        }
        isOccupied = false; // Mark the slot as unoccupied
        SetColorDefault();

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
