using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlanchetteController : MonoBehaviour 
{
    private Coroutine moveCoroutine;

    public Action StartAutoMovement;

    public float planchetteIncreaseAmount = 0.2f; // Amount to increase the planchette speed

    private float planchetteSpeed = 1f; // Speed of the planchette movement
    private int numPositions = 0;
    private const int maxPositions = 5; // Maximum number of positions to move to

    private const float PLANCHEETE_MAX_SPEED = 0.1f;

    private Draggable draggable;
    private CountDownTimer countDownTimer;

    private bool AtMaxSpeed = false;

    IEnumerator WaitAndInvoke(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        StartAutoMovement?.Invoke();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        draggable = GetComponent<Draggable>();
        draggable.DraggingStateChanged += OnDraggingStateChanged;
        countDownTimer = GetComponent<CountDownTimer>();
        countDownTimer.Begin();
        countDownTimer.Finished += IncreasePlanchetteSpeed;
    }

    private void OnDraggingStateChanged(bool isDragging)
    {
        if (!isDragging)
        {
            StartCoroutine(WaitAndInvoke(1f));
        }
        else
        {
            StopMovement();
        }
    }

    public void StopMovement()
    {
        StopCoroutine(moveCoroutine); // Stop the auto movement
    }

    void IncreasePlanchetteSpeed()
    {
        if (!AtMaxSpeed && planchetteSpeed <= PLANCHEETE_MAX_SPEED)
        {
            AtMaxSpeed = true;
            planchetteSpeed = 0.1f;
            numPositions = 0; // Reset the position count
            GameManager.Instance.PlayerPendingDeath(); // Notify the game manager about player death
            return;
        }

        planchetteSpeed -= planchetteIncreaseAmount; // Increase speed every 10 positions
        countDownTimer.Stop();
        countDownTimer.Begin();
    }

    public void SetNewPosition(Vector3 position)
    {
        // Stop any ongoing movement
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveToPosition(position, planchetteSpeed));
    }

    IEnumerator MoveToPosition(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsed = 0f;

        // Keep the z position unchanged
        targetPosition.z = startPosition.z;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        StartAutoMovement?.Invoke();

    }

}
