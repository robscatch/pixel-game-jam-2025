using UnityEngine;

[CreateAssetMenu(fileName = "BoardStats", menuName = "Scriptable Objects/BoardStats")]
public class BoardStats : ScriptableObject
{
    public bool DoesBoardControlPlanchette = false;
    public bool DoesBoardControlCandle = false;


    public int ChanceToBlowOutCandle = 0;
}
