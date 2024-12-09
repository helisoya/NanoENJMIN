using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The color targets
/// </summary>
public enum ColorTarget
{
    ALL,
    PLAYER1,
    PLAYER2
}


public class GameManager : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private Material[] playerMaterials;

    public static GameManager instance;

    public List<Player> players { get; private set; }
    public bool InGame { get; private set; }
    private List<int> readyUps;


    void Awake()
    {
        instance = this;
        InGame = false;
        players = new List<Player>();
        readyUps = new List<int>();
    }

    /// <summary>
    /// Shows the total score on screen
    /// </summary>
    public void ShowTotalScore()
    {
        int total = 0;
        foreach (Player player in players)
        {
            total += player.Score;
        }
        GameGUI.instance.SetScore(total);
    }

    /// <summary>
    /// Registers a new player
    /// </summary>
    /// <param name="player">The new player</param>
    /// <returns>The new player's ID</returns>
    public int RegisterPlayer(Player player)
    {
        int ID = players.Count;
        players.Add(player);
        player.transform.position = spawnPositions[ID].position;
        return ID;
    }

    /// <summary>
    /// Ready up a player
    /// </summary>
    /// <param name="ID">The player's ID</param>
    /// <param name="GUIID">The player's GUIID</param>
    public void ReadyUp(int ID)
    {
        if (!readyUps.Contains(ID))
        {
            readyUps.Add(ID);
            GameGUI.instance.ReadyUpPlayer(ID);
            if (readyUps.Count >= players.Count)
            {
                InGame = true;
                GameGUI.instance.OpenGamePlayScreen();
            }
        }
    }

    /// <summary>
    /// Gets a ColorTarget's material
    /// </summary>
    /// <param name="color">The ColorTarget</param>
    /// <returns>The ColorTarget's material</returns>
    public Material GetPlayerMaterial(ColorTarget color)
    {
        return playerMaterials[(int)color];
    }
}