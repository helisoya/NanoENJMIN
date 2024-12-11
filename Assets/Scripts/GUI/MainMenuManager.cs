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

    [Header("Audio")]
    [SerializeField] private AudioClip[] buttonClips;
    [SerializeField] private AudioClip startClip;

    public void Click_Quit()
    {
        AudioManager.instance.PlaySFX2D(buttonClips[Random.Range(0, buttonClips.Length)]);
        Application.Quit();
    }

    public void Click_Start()
    {
        AudioManager.instance.PlaySFX2D(startClip);
        Transition.LoadSceneWithTransition("Game");
        //SceneManager.LoadScene("Game");
    }

    public void Click_Settings()
    {
        AudioManager.instance.PlaySFX2D(buttonClips[Random.Range(0, buttonClips.Length)]);
        settings.Open();
    }

    public void Click_Credits()
    {
        AudioManager.instance.PlaySFX2D(buttonClips[Random.Range(0, buttonClips.Length)]);
        creditsRoot.SetActive(true);
        generalRoot.SetActive(false);
    }

    public void Click_General()
    {
        AudioManager.instance.PlaySFX2D(buttonClips[Random.Range(0, buttonClips.Length)]);
        creditsRoot.SetActive(false);
        generalRoot.SetActive(true);
    }
}
