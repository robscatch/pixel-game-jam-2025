using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlanchetteController : MonoBehaviour, IPointerClickHandler
{
    Camera m_Camera;

    private bool _IsDragging;
    private bool _IsMoving;
    private Coroutine moveCoroutine;

    public Action OnDragging;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Planchette clicked!"); // Log the click event
        _IsDragging = !_IsDragging; // Toggle dragging state
        if (_IsDragging)
        {
            Debug.Log("Planchette is now being dragged.");
            OnDragging?.Invoke();
        }
        else
        {
            Debug.Log("Planchette is no longer being dragged.");
        }
    }

    void Awake()
    {
        m_Camera = Camera.main;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void SetNewPosition(Vector3 position)
    {
        
        _IsDragging = false; // Disable dragging
        _IsMoving = true; // Set moving state to true
        // Stop any ongoing movement
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveToPosition(position, 1.0f)); // 1.0f seconds duration
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
        _IsMoving = false; // Reset moving state
    }

    void Update()
    {
        if (_IsMoving)
        {
            // If the planchette is moving, we don't want to do anything else
            return;
        }

        if (_IsDragging)
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            Vector3 worldPos = m_Camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, m_Camera.nearClipPlane));
            transform.position = new Vector3(worldPos.x, worldPos.y, transform.position.z);
        }

    }
}
