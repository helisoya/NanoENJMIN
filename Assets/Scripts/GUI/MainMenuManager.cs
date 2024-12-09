using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Handles the main menu
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private GameObject generalRoot;

    [Header("Settings")]
    [SerializeField] private SettingsMenu settings;

    [Header("Credits")]
    [SerializeField] private GameObject creditsRoot;

    public void Click_Quit()
    {
        Application.Quit();
    }

    public void Click_Start()
    {
        SceneManager.LoadScene("Game");
    }

    public void Click_Settings()
    {
        settings.Open();
    }

    public void Click_Credits()
    {
        creditsRoot.SetActive(true);
        generalRoot.SetActive(false);
    }

    public void Click_General()
    {
        creditsRoot.SetActive(false);
        generalRoot.SetActive(true);
    }
}
