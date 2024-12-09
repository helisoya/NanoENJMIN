using TMPro;
using UnityEngine;

/// <summary>
/// Represents an entry in the Leaderboard
/// </summary>
public class LeaderboardEntryGUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private TextMeshProUGUI totalScoreText;

    public void Init(string username, int score1, int score2)
    {
        usernameText.text = username;
        totalScoreText.text = (score1 + score2).ToString();
    }
}
