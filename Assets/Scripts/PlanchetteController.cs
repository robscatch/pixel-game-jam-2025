using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlanchetteController : MonoBehaviour, IPointerClickHandler
{
    Camera m_Camera;

    private bool _IsDragging;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Planchette clicked!"); // Log the click event
        _IsDragging = !_IsDragging; // Toggle dragging state
    }

    void Awake()
    {
        m_Camera = Camera.main;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

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
