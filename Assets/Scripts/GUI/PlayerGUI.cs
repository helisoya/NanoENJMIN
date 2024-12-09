using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents a player's GUI
/// </summary>
public class PlayerGUI : MonoBehaviour
{
    [SerializeField] private HorizontalLayoutGroup layout;
    [SerializeField] private GameObject[] healthBar;
    [SerializeField] private Image manaFill;

    void Start()
    {
        SetPlayerHealth(50);
        SetPlayerManaFill(1);
    }

    /// <summary>
    /// Sets the health's position
    /// </summary>
    /// <param name="value">Health is on the left ?</param>
    public void SetHealthOnLeft(bool value)
    {
        layout.childAlignment = value ? TextAnchor.MiddleLeft : TextAnchor.MiddleRight;
        manaFill.fillOrigin = value ? 0 : 1;
    }

    /// <summary>
    /// Sets the player's health
    /// </summary>
    /// <param name="health">The player's health</param>
    public void SetPlayerHealth(int health)
    {
        for (int i = 0; i < healthBar.Length; i++)
        {
            healthBar[i].SetActive(i < health);
        }
    }

    /// <summary>
    /// Sets the player's color
    /// </summary>
    /// <param name="color">The color</param>
    public void SetManaColor(Color color)
    {
        manaFill.color = color;
    }

    /// <summary>
    /// Sets the player's mana fill amount
    /// </summary>
    /// <param name="manaFillAmount">The player's mana fill amount</param>
    public void SetPlayerManaFill(float manaFillAmount)
    {
        manaFill.fillAmount = manaFillAmount;
    }

}