using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Represents the Game's GUI
/// </summary>
public class GameGUI : MonoBehaviour
{
    public static GameGUI instance { get; private set; }

    [Header("Ready Up")]
    [SerializeField] private GameObject readyUpScreen;
    [SerializeField] private PlayerReadyUp[] playerReadyUps;

    [Header("Players")]
    [SerializeField] private PlayerGUI prefabPlayerGUI;
    [SerializeField] private Transform[] playersGUIRoots;
    [SerializeField] private Color[] playerColors;
    private List<PlayerGUI> playersGUI;

    [Header("General")]
    [SerializeField] private GameObject gameplayScreen;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private EventSystem eventSystem;

    [Header("End Screen")]
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject buttonEnding;
    [SerializeField] private PlayerScore[] scores;

    [Header("Pause Screen")]
    [SerializeField] private GameObject pauseScreen;

    void Awake()
    {
        instance = this;
        playersGUI = new List<PlayerGUI>();
        OpenReadyUpScreen();
    }

    /// <summary>
    /// Toggle the pause menu
    /// </summary>
    public void TogglePauseMenu()
    {
        pauseScreen.SetActive(!pauseScreen.activeInHierarchy);
        Time.timeScale = pauseScreen.activeInHierarchy ? 0 : 1;
    }

    /// <summary>
    /// Opens the end screen
    /// </summary>
    /// <param name="players">The players</param>
    public void OpenEndScreen(List<Player> players)
    {
        gameplayScreen.SetActive(false);
        endScreen.SetActive(true);
        eventSystem.SetSelectedGameObject(buttonEnding);
        eventSystem.firstSelectedGameObject = buttonEnding;

        for (int i = 0; i < 4; i++)
        {
            if (i < players.Count)
            {
                scores[i].SetScore(players[i].Score);
                scores[i].SetColor(playerColors[players[i].ID]);
            }
            else
            {
                scores[i].gameObject.SetActive(false);
            }
        }

    }

    /// <summary>
    /// Sets the timer's value
    /// </summary>
    /// <param name="remainingSeconds">The remaining timer</param>
    public void SetTimerValue(int remainingSeconds)
    {
        timer.text = remainingSeconds + " seconds";
    }

    /// <summary>
    /// Opens the ready up screen
    /// </summary>
    public void OpenReadyUpScreen()
    {
        readyUpScreen.SetActive(true);
        gameplayScreen.SetActive(false);

        foreach (PlayerReadyUp readyUp in playerReadyUps)
        {
            readyUp.gameObject.SetActive(false);
            readyUp.SetReadyUpCheckActive(false);
        }
    }

    /// <summary>
    /// Opens the gameplay screen
    /// </summary>
    public void OpenGamePlayScreen()
    {
        readyUpScreen.SetActive(false);
        gameplayScreen.SetActive(true);
    }

    /// <summary>
    /// Ready ups a player
    /// </summary>
    /// <param name="ID">The player's ID</param>
    public void ReadyUpPlayer(int ID)
    {
        playerReadyUps[ID].SetReadyUpCheckActive(true);
    }

    /// <summary>
    /// Adds a new player's GUI
    /// </summary>
    /// <param name="ID">The player's ID</param>
    /// <returns></returns>
    public int AddNewPlayerGUI(int ID)
    {
        int GUIID = playersGUI.Count;
        playerReadyUps[ID].gameObject.SetActive(true);
        playerReadyUps[ID].SetPlayerColor(playerColors[ID]);
        PlayerGUI player = Instantiate(prefabPlayerGUI, playersGUIRoots[GUIID]);
        player.SetHealthOnLeft(ID % 2 == 0);
        player.SetManaColor(playerColors[ID]);
        playersGUI.Add(player);
        return GUIID;
    }

    /// <summary>
    /// Sets a player's mana fill amount
    /// </summary>
    /// <param name="playerID">The player's ID</param>
    /// <param name="manaFill">The player's mana fill amount</param>
    public void SetPlayerManaFill(int playerID, float manaFill)
    {
        playersGUI[playerID].SetPlayerManaFill(manaFill);
    }

    /// <summary>
    /// Sets a player's health
    /// </summary>
    /// <param name="playerID">The player's ID</param>
    /// <param name="health">The player's health</param>
    public void SetPlayerHealth(int playerID, int health)
    {
        playersGUI[playerID].SetPlayerHealth(health);
    }


    /// <summary>
    /// Quits the gameplay scene
    /// </summary>
    public void Click_ToMainMenu()
    {
        // Quit
    }
}