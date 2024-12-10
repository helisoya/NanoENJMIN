using TMPro;
using UnityEngine;

/// <summary>
/// Represents an entry in the Leaderboard
/// </summary>
public class LeaderboardEntryGUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private TextMeshProUGUI totalScoreText;

    public void Init(string username, int score)
    {
        usernameText.text = username;
        totalScoreText.text = score.ToString();
    }
}
