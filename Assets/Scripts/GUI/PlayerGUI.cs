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
    [SerializeField] private Image[] healthBar;
    [SerializeField] private Image manaFill;
    private Sprite spriteAlive;
    private Sprite spriteDead;

    void Start()
    {
        SetPlayerHealth(50);
        SetPlayerManaFill(1);
    }

    /// <summary>
    /// Sets the GUI's sprites
    /// </summary>
    /// <param name="alive">The sprite for the alive heart</param>
    /// <param name="dead">The sprite for the dead heart</param>
    public void SetSprites(Sprite alive, Sprite dead)
    {
        spriteAlive = alive;
        spriteDead = dead;
        SetPlayerHealth(50);
    }



    /// <summary>
    /// Sets the player's health
    /// </summary>
    /// <param name="health">The player's health</param>
    public void SetPlayerHealth(int health)
    {
        for (int i = healthBar.Length - 1; i >= 0; i--)
        {
            print(i);
            healthBar[i].sprite = i < health ? spriteAlive : spriteDead;
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