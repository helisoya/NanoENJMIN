using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Handles the game's sounds
/// </summary>
public class AudioManager : MonoBehaviour
{
    [Header("Infos")]
    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfx2DSource;

    public static AudioManager instance { get; private set; }

    void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Plays a background music
    /// </summary>
    /// <param name="clip">The music</param>
    public void PlayBGM(AudioClip clip)
    {
        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.Play();
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
