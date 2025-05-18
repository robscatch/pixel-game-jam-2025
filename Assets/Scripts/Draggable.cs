using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Draggable : MonoBehaviour, IPointerClickHandler
{
    Camera m_Camera;

    private bool _IsDragging;
    public Action<bool> DraggingStateChanged;
    public Action<Transform> DragEnded;
    public Action<Transform> DragStarted;

    public bool IsDragging { get => _IsDragging;  }

    private Vector3 startposition;

    public void OnPointerClick(PointerEventData eventData)
    {
        _IsDragging = !_IsDragging; // Toggle dragging state
        Debug.Log("Dragging state changed: " + _IsDragging); // Log the dragging state change
        DraggingStateChanged?.Invoke(_IsDragging); // Notify listeners of the dragging state change

        if (!_IsDragging)
        {
            DragEnded?.Invoke(transform); // Notify listeners that dragging has ended
        }
        else
        {
            DragStarted?.Invoke(transform); // Notify listeners that dragging has started
        }
    }

    public void ResetPosition()
    {
        transform.position = startposition; // Reset the position of the object
    }

    void Awake()
    {
        m_Camera = Camera.main;
        startposition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_IsDragging)
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            Vector3 worldPos = m_Camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, m_Camera.nearClipPlane));
            transform.position = new Vector3(worldPos.x, worldPos.y, transform.position.z);
        }

    }
}
