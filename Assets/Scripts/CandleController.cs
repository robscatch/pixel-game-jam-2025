using UnityEngine;
using UnityEngine.EventSystems;

public class CandleController : BoardStatsUser, IPointerClickHandler
{

    [SerializeField]
    private GameObject flame; // Prefab for the candle

    private bool isFlameOn = false; // Flag to check if the flame is on or off


    private CountDownTimer _countDownTimer; // Reference to the countdown timer

    public bool IsFlameOn { get => isFlameOn; }

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

        _countDownTimer.Finished += BlowOutCandle; // Subscribe to the Finished event of the countdown timer
        flame.SetActive(false); // Start with the flame inactive
    }

    private void BlowOutCandle()
    {
        if (IsFlameOn)
        {
            flame.SetActive(false); // Turn off the flame
            isFlameOn = false;
            _countDownTimer.Stop(); // Stop the countdown timer
        }
    } 

    private void ToggleCandle()
    {
        if (IsFlameOn)
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

    public override void OnBoardStatsSet()
    {
        BlowOutCandle(); // Call the method to blow out the candle
        _countDownTimer.duration = boardStats.TimeToBlowOutCandleInSeconds; // Set the countdown timer duration
    }
}
