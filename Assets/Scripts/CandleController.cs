using UnityEngine;
using UnityEngine.EventSystems;

public class CandleController : MonoBehaviour, IPointerClickHandler
{

    [SerializeField]
    private GameObject flame; // Prefab for the candle

    private bool isFlameOn = false; // Flag to check if the flame is on or off

    [SerializeField]
    private BoardStats _boardStats; // Reference to the BoardStats scriptable object

    private CountDownTimer _countDownTimer; // Reference to the countdown timer

    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleCandle(); // Call the method to light the candle when clicked
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _countDownTimer = GetComponent<CountDownTimer>(); // Get the CountDownTimer component attached to this GameObject
        if (_countDownTimer == null)
        {
            Debug.LogError("CountDownTimer not found in the scene.");
        }

        _countDownTimer.duration = _boardStats.TimeToBlowOutCandleInSeconds; // Set the countdown timer duration
        _countDownTimer.Finished += BlowOutCandle; // Subscribe to the Finished event of the countdown timer
        flame.SetActive(false); // Start with the flame inactive
    }

    private void BlowOutCandle()
    {
        if (isFlameOn)
        {
            flame.SetActive(false); // Turn off the flame
            isFlameOn = false;
            _countDownTimer.Stop(); // Stop the countdown timer
        }
    } 

    private void ToggleCandle()
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
            _countDownTimer.Begin();
        }
    }
}
