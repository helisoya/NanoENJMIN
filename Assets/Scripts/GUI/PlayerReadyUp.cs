using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Represents a player's Ready up component
/// </summary>
public class PlayerReadyUp : MonoBehaviour
{
    [SerializeField] private GameObject readyUpCheck;
    [SerializeField] private TextMeshProUGUI playerText;

    /// <summary>
    /// Sets the ready up check active or not
    /// </summary>
    /// <param name="value">Is the check active ?</param>
    public void SetReadyUpCheckActive(bool value)
    {
        readyUpCheck.SetActive(value);
    }

    /// <summary>
    /// Sets the player's color
    /// </summary>
    /// <param name="color">The player's color</param>
    public void SetPlayerColor(Color color)
    {
        playerText.color = color;
    }
}