using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlanchetteController : MonoBehaviour 
{
    private Coroutine moveCoroutine;

    public Action StartAutoMovement;

    private float planchetteSpeed = 1f; // Speed of the planchette movement
    private int numPositions = 0;
    private const int maxPositions = 5; // Maximum number of positions to move to

    private Draggable draggable;

    IEnumerator WaitAndInvoke(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        StartAutoMovement?.Invoke();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartAutoMovement?.Invoke();
        draggable = GetComponent<Draggable>();
        draggable.DraggingStateChanged += OnDraggingStateChanged;
    }

    private void OnDraggingStateChanged(bool isDragging)
    {
        if (!isDragging)
        {
            StartCoroutine(WaitAndInvoke(1f));
        }
        else
        {
            StopCoroutine(moveCoroutine); // Stop the auto movement when dragging starts
        }
    }

    public void SetNewPosition(Vector3 position)
    {
        if (numPositions > maxPositions && planchetteSpeed > .1)
        {
            planchetteSpeed -= 0.1f; // Increase speed every 10 positions
            numPositions = 0; // Reset the position count
            Debug.Log("Planchette speed increased to: " + planchetteSpeed);
        }

        numPositions++;

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
