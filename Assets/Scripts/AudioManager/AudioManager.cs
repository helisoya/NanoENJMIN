using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Handles the game's sounds
/// </summary>
public class AudioManager : MonoBehaviour
{
    [Header("Infos")]
    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField] private float bgmTransitionSpeed = 1f;
    [SerializeField] private AudioSource[] bgms;
    [SerializeField] private AudioSource sfx2DSource;
    [SerializeField] private AudioSource ambSource;

    public static AudioManager instance { get; private set; }

    void Awake()
    {
        instance = this;
    }


    /// <summary>
    /// Stops the AMB
    /// </summary>
    public void StopAMB()
    {
        ambSource.volume = 0f;
    }

    /// <summary>
    /// Stops the BGM
    /// </summary>
    public void StopBGM()
    {
        for (int i = 0; i < bgms.Length; i++)
        {
            bgms[i].volume = 0f;
        }
    }

    /// <summary>
    /// Enables a BGM
    /// </summary>
    /// <param name="idx">The BGM's index</param>
    /// <param name="instant">Should the transition be instantanious ?</param>
    public void EnableBGM(int idx, bool instant = false)
    {
        if (bgms.Length <= idx) return;

        if (instant)
        {
            bgms[idx].volume = 1f;
        }
        else
        {
            StartCoroutine(Routine_EnableBGM(idx));
        }
    }

    IEnumerator Routine_EnableBGM(int idx)
    {
        AudioSource target = bgms[idx];
        while (target.volume < 1f)
        {
            target.volume = Mathf.Clamp(target.volume + Time.deltaTime * bgmTransitionSpeed, 0f, 1f);
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Plays a 2D SFX
    /// </summary>
    /// <param name="clip">The SFX</param>
    public void PlaySFX2D(AudioClip clip)
    {
        sfx2DSource.PlayOneShot(clip);
    }

    /// <summary>
    /// Plays a 3D SFX
    /// </summary>
    /// <param name="clip">The SFX</param>
    /// <param name="position">The SFX's position</param>
    /// <param name="parent">The SFX's parent (optional)</param>
    public void PlaySFX3D(AudioClip clip, Vector3 position, Transform parent = null)
    {
        GameObject obj = Instantiate(new GameObject(), position, Quaternion.identity, parent);
        AudioSource source = obj.AddComponent<AudioSource>();

        source.clip = clip;
        source.spatialBlend = 1f;
        source.outputAudioMixerGroup = sfxGroup;
        source.Play();

        Destroy(obj, clip.length);
    }
}
