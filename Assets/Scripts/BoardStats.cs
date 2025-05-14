using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoardStats", menuName = "Scriptable Objects/BoardStats")]
public class BoardStats : ScriptableObject
{
    public int TimeToBlowOutCandleInSeconds = 0;

    public List<CrystalType> CrystalTypes;
}
