using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Represents a player's score in the end screen
/// </summary>
public class PlayerScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI playerText;

    /// <summary>
    /// Sets the component's score
    /// </summary>
    /// <param name="score">The score</param>
    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }

    /// <summary>
    /// Sets the player's color
    /// </summary>
    /// <param name="color">The player's color</param>
    public void SetColor(Color color)
    {
        playerText.color = color;
    }
}