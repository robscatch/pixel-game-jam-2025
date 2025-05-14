using System;
using UnityEngine;

public abstract class BoardStatsUser: MonoBehaviour
{
    public BoardStats boardStats; // Reference to the BoardStats scriptable object

    public void SetBoardStats(BoardStats newBoardStats)
    {
        boardStats = newBoardStats;
        OnBoardStatsSet();
    }
    public BoardStats GetBoardStats()
    {
        return boardStats;
    }

    public abstract void OnBoardStatsSet(); // Abstract method to be implemented by derived classes
}
