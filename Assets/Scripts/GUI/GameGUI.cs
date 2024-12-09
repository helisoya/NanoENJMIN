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
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private EventSystem eventSystem;

    [Header("Lose Screen")]
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private TextMeshProUGUI loseScoreText;

    [Header("End Screen")]
    [SerializeField] private GameObject endScreen;
    [SerializeField] private TextMeshProUGUI endScoreText;
    [SerializeField] private TMP_InputField endNameInputField;
    [SerializeField] private PlayerScore[] scores;

    [Header("Leaderboard")]
    [SerializeField] private GameObject leaderboardScreen;
    [SerializeField] private Transform leaderboardRoot;
    [SerializeField] private LeaderboardEntryGUI leaderboardEntryPrefab;


    [Header("Pause Screen")]
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private SettingsMenu settings;

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
        if (!pauseScreen.activeInHierarchy) settings.Close();
    }

    /// <summary>
    /// Opens the end screen
    /// </summary>
    /// <param name="players">The players</param>
    public void OpenEndScreen(List<Player> players)
    {
        gameplayScreen.SetActive(false);
        endScreen.SetActive(true);

        int total = 0;

        for (int i = 0; i < scores.Length; i++)
        {
            if (i < players.Count)
            {
                total += players[i].Score;
                scores[i].SetScore(players[i].Score);
                scores[i].SetColor(playerColors[players[i].ID]);
            }
            else
            {
                scores[i].gameObject.SetActive(false);
            }
        }

        endScoreText.text = "Score : " + total;
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
    /// Sets the score text
    /// </summary>
    /// <param name="score">The score to show</param>
    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }


    /// <summary>
    /// Opens the leaderboard
    /// </summary>
    void OpenLeaderboard()
    {
        endScreen.SetActive(false);
        leaderboardScreen.SetActive(true);

        List<LeaderboardEntry> entries = LeaderboardManager.instance.leaderboard.entries;

        foreach (LeaderboardEntry entry in entries)
        {
            Instantiate(leaderboardEntryPrefab, leaderboardRoot).Init(entry.entryName, entry.player1Score, entry.player2Score);
        }


        leaderboardRoot.GetComponent<RectTransform>().sizeDelta = new Vector2(
            leaderboardRoot.GetComponent<RectTransform>().sizeDelta.x,
            (leaderboardEntryPrefab.GetComponent<RectTransform>().sizeDelta.y + 5) * entries.Count
        );
    }

    /// <summary>
    /// Opens the death screen
    /// </summary>
    /// <param name="finalScore">The final score</param>
    public void OpenDeathScreen(int finalScore)
    {
        gameplayScreen.SetActive(false);
        loseScreen.SetActive(true);
        loseScoreText.text = "Score : " + finalScore;
    }

    public void Click_ToLeaderboard()
    {
        // Quit
        if (!string.IsNullOrEmpty(endNameInputField.text))
        {
            GameManager.instance.SaveScore(endNameInputField.text);
            OpenLeaderboard();
        }
    }

    public void Click_Quit()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Click_Retry()
    {
        SceneManager.LoadScene("Game");
    }

    public void Click_Continue()
    {
        TogglePauseMenu();
    }

    public void Click_Settings()
    {
        settings.Open();
    }
}