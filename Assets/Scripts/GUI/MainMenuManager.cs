using UnityEngine;
using UnityEngine.EventSystems;
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

    [Header("Event System")]
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject generalObj;
    [SerializeField] private GameObject creditsObj;
    [SerializeField] private GameObject settingsObj;
    [SerializeField] private GameObject generalAfterSettingsObj;
    [SerializeField] private GameObject generaleAfterCreditsObj;


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
        eventSystem.SetSelectedGameObject(settingsObj);
    }

    public void Click_CloseSettings()
    {
        AudioManager.instance.PlaySFX2D(buttonClips[Random.Range(0, buttonClips.Length)]);
        settings.Close();
        eventSystem.SetSelectedGameObject(generalAfterSettingsObj);
    }

    public void Click_Credits()
    {
        AudioManager.instance.PlaySFX2D(buttonClips[Random.Range(0, buttonClips.Length)]);
        creditsRoot.SetActive(true);
        generalRoot.SetActive(false);

        eventSystem.SetSelectedGameObject(creditsObj);
    }

    public void Click_General()
    {
        AudioManager.instance.PlaySFX2D(buttonClips[Random.Range(0, buttonClips.Length)]);
        creditsRoot.SetActive(false);
        generalRoot.SetActive(true);

        eventSystem.SetSelectedGameObject(generaleAfterCreditsObj);
    }
}
