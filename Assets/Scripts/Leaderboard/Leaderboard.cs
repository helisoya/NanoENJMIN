using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Leaderboard
{
    public List<LeaderboardEntry> entries;

    public Leaderboard()
    {
        entries = new List<LeaderboardEntry>();
    }
}

[System.Serializable]
public class LeaderboardEntry
{

    public string entryName;
    public int player1Score;
    public int player2Score;

    public LeaderboardEntry(string entryName, int player1Score, int player2Score)
    {
        this.entryName = entryName;
        this.player1Score = player1Score;
        this.player2Score = player2Score;
    }
}

