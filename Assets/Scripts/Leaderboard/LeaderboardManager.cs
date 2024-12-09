using System.IO;
using UnityEngine;

public class LeaderboardManager
{
    private static LeaderboardManager _instance;
    public static LeaderboardManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new LeaderboardManager();
            }
            return _instance;
        }
    }

    public Leaderboard leaderboard { get; private set; }
    private string savePath = FileManager.savPath + "leaderboard.caribou";


    public LeaderboardManager()
    {
        leaderboard = new Leaderboard();
        savePath = FileManager.savPath + "leaderboard.caribou";
        Load();
    }


    /// <summary>
    /// Adds an entry to the leaderboard
    /// </summary>
    /// <param name="name">The entry's name</param>
    /// <param name="scorePlayer1">The player 1 score</param>
    /// <param name="scorePlayer2">The player 2 score</param>
    public void AddEntry(string name, int scorePlayer1, int scorePlayer2)
    {
        leaderboard.entries.Add(new LeaderboardEntry(name, scorePlayer1, scorePlayer2));
        Save();
    }

    /// <summary>
    /// Saves the leaderboard
    /// </summary>
    private void Save()
    {
        FileManager.SaveJSON(savePath, leaderboard);
    }

    /// <summary>
    /// Loads a leaderboard
    /// </summary>
    private void Load()
    {
        if (File.Exists(savePath))
        {
            leaderboard = FileManager.LoadJSON<Leaderboard>(savePath);
        }
        else
        {
            Save();
        }
    }

}
