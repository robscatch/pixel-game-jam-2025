using UnityEngine;

public class CrystalController : MonoBehaviour
{

    [SerializeField] 
    private CrystalType crystalType;

    public CrystalType CrystalType { get => crystalType; }
}
