using UnityEngine;
using UnityEngine.EventSystems;

public class CandleController : MonoBehaviour, IPointerClickHandler
{

    [SerializeField]
    private GameObject flame; // Prefab for the candle

    private bool isFlameOn = false; // Flag to check if the flame is on or off

    [SerializeField]
    private BoardStats _boardStats; // Reference to the BoardStats scriptable object

    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleCandle(); // Call the method to light the candle when clicked
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flame.SetActive(false); // Start with the flame inactive
    }

    public bool isLit()
    {
        return isFlameOn; // Return the current state of the flame
    }


    public void ToggleCandle()
    {
        if (isFlameOn)
        {
            flame.SetActive(false); // Turn off the flame
            isFlameOn = false;
        }
        else
        {
            flame.SetActive(true); // Turn on the flame
            isFlameOn = true;
        }
    }
    void Update()
    {
        //check this only once a second
        if (Time.frameCount % 60 == 0)
        {
            if (isLit() && _boardStats.DoesBoardControlCandle)
            {
                //Randomly blow out the candle
                if (Random.Range(0, 100) < _boardStats.ChanceToBlowOutCandle)
                {
                    Debug.Log("Candle blown out!");
                    ToggleCandle();
                }
            }
        }
    }
}
