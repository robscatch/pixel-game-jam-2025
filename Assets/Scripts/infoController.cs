using UnityEngine;
using UnityEngine.EventSystems;

public class infoController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer; // Reference to the SpriteRenderer component

    [SerializeField]
    private AudioClip _infoSound; // Reference to the sound clip to play

    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySingle(_infoSound); // Play the sound when clicked
        _spriteRenderer.enabled = !_spriteRenderer.enabled; // Toggle the visibility of the sprite when clicked
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRenderer.enabled = false; // Disable the SpriteRenderer at the start

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
