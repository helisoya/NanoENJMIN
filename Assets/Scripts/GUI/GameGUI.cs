using System.Collections;
using System.Collections.Generic;
using Dan.Main;
using Dan.Models;
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

    [Header("End Screen")]
    [SerializeField] private GameObject endScreen;
    [SerializeField] private TextMeshProUGUI endScoreText;
    [SerializeField] private TMP_InputField endNameInputField;
    [SerializeField] private PlayerScore[] scores;
    [SerializeField] private Transform leaderboardRoot;
    [SerializeField] private LeaderboardEntryGUI leaderboardEntryPrefab;
    [SerializeField] private GameObject winOnlyWidgets;
    [SerializeField] private GameObject leaderboardDataSender;
    [SerializeField] private TextMeshProUGUI endScreenTitle;
    private int totalScoreForEndScreen;


    [Header("Pause Screen")]
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private SettingsMenu settings;

    [Header("Audio")]
    [SerializeField] private AudioClip[] buttonClips;

    [Header("Event System")]
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject pauseObj;
    [SerializeField] private GameObject pauseAfterSettingsObj;
    [SerializeField] private GameObject endObj;
    [SerializeField] private GameObject settingsObj;
    [SerializeField] private float stopReceivingInputsFor = 0.25f;
    private float stopStart;

    public bool InMenu { get { return pauseScreen.activeInHierarchy; } }

    void Awake()
    {
        instance = this;
        playersGUI = new List<PlayerGUI>();
        OpenReadyUpScreen();
    }

    /// <summary>
    /// Plays a button clip
    /// </summary>
    private void PlayButtonClip()
    {
        AudioManager.instance.PlaySFX2D(buttonClips[Random.Range(0, buttonClips.Length)]);
    }

    /// <summary>
    /// Toggle the pause menu
    /// </summary>
    public void TogglePauseMenu()
    {
        if (Time.unscaledTime - stopStart < stopReceivingInputsFor) return;

        stopStart = Time.unscaledTime;

        PlayButtonClip();
        pauseScreen.SetActive(!pauseScreen.activeInHierarchy);
        Time.timeScale = pauseScreen.activeInHierarchy ? 0 : 1;
        if (!pauseScreen.activeInHierarchy) settings.Close();
        else eventSystem.SetSelectedGameObject(pauseObj);
    }

    /// <summary>
    /// Opens the end screen
    /// </summary>
    /// <param name="players">The players</param>
    /// <param name="hasWon">Have the players won ?</param>
    public void OpenEndScreen(List<Player> players, bool hasWon)
    {
        gameplayScreen.SetActive(false);
        endScreen.SetActive(true);

        totalScoreForEndScreen = 0;

        for (int i = 0; i < scores.Length; i++)
        {
            if (i < players.Count)
            {
                totalScoreForEndScreen += players[i].Score;
                scores[i].SetScore(players[i].Score);
                scores[i].SetColor(playerColors[players[i].ID]);
            }
            else
            {
                scores[i].gameObject.SetActive(false);
            }
        }

        endScoreText.text = "Score : " + totalScoreForEndScreen;

        leaderboardDataSender.SetActive(hasWon);
        winOnlyWidgets.SetActive(hasWon);

        endScreenTitle.text = hasWon ? "You won !" : "Game over";

        eventSystem.SetSelectedGameObject(endObj);

        if (hasWon) RefreshLeaderboard();

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
            readyUp.SetPlayerActive(false);
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
        PlayButtonClip();
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
        playerReadyUps[ID].SetPlayerActive(true);
        playerReadyUps[ID].SetPlayerColor(playerColors[ID]);
        PlayerGUI player = Instantiate(prefabPlayerGUI, playersGUIRoots[GUIID]);
        player.SetHealthOnLeft(ID % 2 == 0);
        player.SetManaColor(playerColors[ID]);
        playersGUI.Add(player);
        PlayButtonClip();
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
    /// Refreshes the leaderboard
    /// </summary>
    public void RefreshLeaderboard()
    {
        Leaderboards.NanoPoulpeLeaderboard.GetEntries((msg) =>
        {
            foreach (Transform child in leaderboardRoot)
            {
                Destroy(child.gameObject);
            }

            foreach (Entry entry in msg)
            {
                Instantiate(leaderboardEntryPrefab, leaderboardRoot).Init(entry.Username, entry.Score);
            }

            leaderboardRoot.GetComponent<RectTransform>().sizeDelta = new Vector2(
                leaderboardRoot.GetComponent<RectTransform>().sizeDelta.x,
                (leaderboardEntryPrefab.GetComponent<RectTransform>().sizeDelta.y + 5) * msg.Length
            );
        });

        /*
        List<LeaderboardEntry> entries = LeaderboardManager.instance.leaderboard.entries;

        foreach (LeaderboardEntry entry in entries)
        {
            Instantiate(leaderboardEntryPrefab, leaderboardRoot).Init(entry.entryName, entry.player1Score, entry.player2Score);
        }


        leaderboardRoot.GetComponent<RectTransform>().sizeDelta = new Vector2(
            leaderboardRoot.GetComponent<RectTransform>().sizeDelta.x,
            (leaderboardEntryPrefab.GetComponent<RectTransform>().sizeDelta.y + 5) * entries.Count
        );
        */
    }

    public void Click_SendData()
    {
        // Quit
        PlayButtonClip();
        if (!string.IsNullOrEmpty(endNameInputField.text))
        {
            leaderboardDataSender.SetActive(false);

            Leaderboards.NanoPoulpeLeaderboard.ResetPlayer(() =>
                {
                    Leaderboards.NanoPoulpeLeaderboard.UploadNewEntry(
                        endNameInputField.text,
                        totalScoreForEndScreen,
                        (msg) =>
                        {
                            Leaderboards.NanoPoulpeLeaderboard.ResetPlayer();
                            RefreshLeaderboard();
                        }
                    );
                });
        }
    }

    public void Click_Quit()
    {
        if (Time.unscaledTime - stopStart < stopReceivingInputsFor) return;

        stopStart = Time.unscaledTime;

        PlayButtonClip();
        Transition.LoadSceneWithTransition("MainMenu");
    }

    public void Click_Retry()
    {
        if (Time.unscaledTime - stopStart < stopReceivingInputsFor) return;

        stopStart = Time.unscaledTime;

        PlayButtonClip();
        Transition.LoadSceneWithTransition(SceneManager.GetActiveScene().buildIndex);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Click_Continue()
    {
        TogglePauseMenu();
    }

    public void Click_Settings()
    {
        if (Time.unscaledTime - stopStart < stopReceivingInputsFor) return;

        stopStart = Time.unscaledTime;

        PlayButtonClip();
        settings.Open();
        eventSystem.SetSelectedGameObject(settingsObj);
    }

    public void Click_CloseSettings()
    {
        if (Time.unscaledTime - stopStart < stopReceivingInputsFor) return;

        stopStart = Time.unscaledTime;

        PlayButtonClip();
        settings.Close();
        eventSystem.SetSelectedGameObject(pauseAfterSettingsObj);
    }
}