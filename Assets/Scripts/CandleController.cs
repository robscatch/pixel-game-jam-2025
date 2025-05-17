using UnityEngine;
using UnityEngine.EventSystems;

public class CandleController : BoardStatsUser, IPointerClickHandler
{

    [SerializeField]
    private AudioClip LightCandleSound;

    [SerializeField]
    private AudioClip BlowOutCandleSound;

    private bool isFlameOn = false; // Flag to check if the flame is on or off

    private CountDownTimer _countDownTimer; // Reference to the countdown timer
    private Animator _animator; // Reference to the animator component

    public bool IsFlameOn { get => isFlameOn; }

    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleCandle(); // Call the method to light the candle when clicked
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        _animator = GetComponent<Animator>(); // Get the Animator component attached to this GameObject
        _countDownTimer = GetComponent<CountDownTimer>(); // Get the CountDownTimer component attached to this GameObject
        if (_countDownTimer == null)
        {
            Debug.LogError("CountDownTimer not found in the scene.");
        }

        _countDownTimer.Finished += BlowOutCandle; // Subscribe to the Finished event of the countdown timer
    }

    private void BlowOutCandle()
    {
        if (IsFlameOn)
        {
            SoundManager.Instance.PlaySingle(BlowOutCandleSound);
            _animator.SetTrigger("BlowOut"); // Trigger the blow out animation
            isFlameOn = false;
            _countDownTimer.Stop(); // Stop the countdown timer
        }
    } 

    private void ToggleCandle()
    {
        if (IsFlameOn)
        {
            SoundManager.Instance.PlaySingle(BlowOutCandleSound);
            _animator.SetTrigger("BlowOut"); // Trigger the blow out animation
            isFlameOn = false;
        }
        else
        {
            SoundManager.Instance.PlaySingle(LightCandleSound);
            _animator.SetTrigger("Light"); // Trigger the light animation
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
