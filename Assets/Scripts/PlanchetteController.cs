using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlanchetteController : MonoBehaviour, IPointerClickHandler
{
    Camera m_Camera;

    private bool _IsDragging;
    private Coroutine moveCoroutine;

    public Action StartAutoMovement;

    private float planchetteSpeed = 1f; // Speed of the planchette movement
    private int numPositions = 0;
    private const int maxPositions = 5; // Maximum number of positions to move to

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Planchette clicked!"); // Log the click event
        _IsDragging = !_IsDragging; // Toggle dragging state
        if (_IsDragging)
        {
            Debug.Log("Planchette is now being dragged.");
        }
        else
        {
            Debug.Log("Planchette is no longer being dragged.");
            StartCoroutine(WaitAndInvoke(1f));
        }
    }

    IEnumerator WaitAndInvoke(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        StartAutoMovement?.Invoke();
    }

    void Awake()
    {
        m_Camera = Camera.main;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartAutoMovement?.Invoke();
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

    void Update()
    {
        if (_IsDragging)
        {
            StopCoroutine(moveCoroutine); // Stop any ongoing movement
            Vector3 mousePos = Mouse.current.position.ReadValue();
            Vector3 worldPos = m_Camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, m_Camera.nearClipPlane));
            transform.position = new Vector3(worldPos.x, worldPos.y, transform.position.z);
        }

    }
}
