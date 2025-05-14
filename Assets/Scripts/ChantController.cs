using UnityEngine;
using UnityEngine.EventSystems;

public class ChantController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private CrystalSlotController _crystalSlotController; // Reference to the CrystalSlotController script
    [SerializeField]
    private PlanchetteController _planchetteController; // Reference to the PlanchetteController script
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_crystalSlotController.AllCorrect())
        {
            Debug.Log("All crystals are correct!"); // Log message when all crystals are correct
            SoundManager.Instance.PlaySingle(null);
            _planchetteController.StopMovement(); // Stop the planchette movement
        }
        else
        {
            Debug.Log("Not all crystals are correct!"); // Log message when not all crystals are correct
            SoundManager.Instance.PlaySingle(null);
        }
    }
}
