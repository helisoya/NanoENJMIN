using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// Represents the settings menu
/// </summary>
public class SettingsMenu : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private GameObject root;

    [Header("Audio")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider sliderMaster;
    [SerializeField] private Slider sliderSFX;
    [SerializeField] private Slider sliderBGM;


    /// <summary>
    /// Opens the settings menu
    /// </summary>
    public void Open()
    {
        root.SetActive(true);

        float value;
        mixer.GetFloat("BGM", out value);
        sliderBGM.SetValueWithoutNotify(value);

        mixer.GetFloat("SFX", out value);
        sliderSFX.SetValueWithoutNotify(value);

        mixer.GetFloat("MASTER", out value);
        sliderMaster.SetValueWithoutNotify(value);
    }

    /// <summary>
    /// Closes the settings menu
    /// </summary>
    public void Close()
    {
        root.SetActive(false);
    }

    public void SetMasterVolume(float value)
    {
        mixer.SetFloat("MASTER", value);
    }

    public void SetBGMVolume(float value)
    {
        mixer.SetFloat("BGM", value);
    }

    public void SetSFXVolume(float value)
    {
        mixer.SetFloat("SFX", value);
    }


}
