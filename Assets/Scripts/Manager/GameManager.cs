using System;
using System.Collections;
using System.Collections.Generic;
using Dan.Main;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// The color targets
/// </summary>
public enum ColorTarget
{
    ALL,
    YELLOW,
    PURPLE
}


public class GameManager : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private Transform[] respawnPositions;
    [SerializeField] private Material[] playerMaterials;
    [SerializeField] private ParticleSystem[] shootPlayerParticles;
    [SerializeField] private ParticleSystem[] hitPlayerParticles;
    [SerializeField] private PlayerInputManager inputManager;

    public static GameManager instance;

    public List<Player> players { get; private set; }
    public bool InGame { get; private set; }
    private List<int> readyUps;

    public Action onGameStarted;


    void Awake()
    {
        instance = this;
        InGame = false;
        players = new List<Player>();
        readyUps = new List<int>();
    }

    public void RespawnPlayer(Player player)
    {
        player.transform.position = respawnPositions[player.ID].position;
        player.ResetBodyRotation();
        player.Respawned(spawnPositions[player.ID].position);
    }

    /// <summary>
    /// Computes the total score
    /// </summary>
    /// <returns>The total score</returns>
    public int GetTotalScore()
    {
        int total = 0;
        foreach (Player player in players)
        {
            total += player.Score;
        }
        return total;
    }

    /// <summary>
    /// Shows the total score on screen
    /// </summary>
    public void ShowTotalScore()
    {
        GameGUI.instance.SetScore(GetTotalScore());
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
                readyUps.Clear();
                inputManager.DisableJoining();
                GameGUI.instance.OpenGamePlayScreen();
                AudioManager.instance.EnableBGM(0);
                onGameStarted.Invoke();
                // Start the timeline
                TimelineManager.instance.Play();
            }
        }
    }

    /// <summary>
    /// Register that a player is dead
    /// </summary>
    /// <param name="ID">The player's ID</param>
    public void KillPlayer(int ID)
    {
        if (!readyUps.Contains(ID))
        {
            readyUps.Add(ID);

            if (readyUps.Count >= players.Count)
            {
                InGame = false;
                readyUps.Clear();
                GameGUI.instance.OpenEndScreen(players, false);
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

    public ParticleSystem GetShootParticles(ColorTarget color)
    {
        return shootPlayerParticles[(int)color];
    }

    public ParticleSystem GetHitParticles(ColorTarget color)
    {
        return hitPlayerParticles[(int)color];
    }

    public Player GetPlayerFromColour(ColorTarget colour)
    {
        foreach (Player player in players)
        {
            if (player.Color == colour)
            {
                return player;
            }
        }

        return null;
    }
}