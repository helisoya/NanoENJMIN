using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    private List<EnemySpawner> _spawners;

    [Header("Audio")]
    [SerializeField] private AudioClip[] deathclips;
    [SerializeField] private AudioClip[] hitClips;
    [SerializeField] private AudioClip[] shieldBreakClip;
    [SerializeField] private AudioClip[] shieldHitClip;
    [SerializeField] private AudioClip[] shieldNoDmgClip;
    [SerializeField] private AudioClip[] shotClip;


    public void PlayDeathClip()
    {
        AudioManager.instance.PlaySFX2D(deathclips[Random.Range(0, deathclips.Length)]);
    }

    public void PlayHitClip()
    {
        AudioManager.instance.PlaySFX2D(hitClips[Random.Range(0, hitClips.Length)]);
    }


    public void PlayShieldBreakClip()
    {
        AudioManager.instance.PlaySFX2D(shieldBreakClip[Random.Range(0, shieldBreakClip.Length)]);
    }


    public void PlayShieldHitClip()
    {
        AudioManager.instance.PlaySFX2D(shieldHitClip[Random.Range(0, shieldHitClip.Length)]);
    }


    public void PlayShieldNoDmgClip()
    {
        AudioManager.instance.PlaySFX2D(shieldNoDmgClip[Random.Range(0, shieldNoDmgClip.Length)]);
    }

    public void PlayShotClip()
    {
        AudioManager.instance.PlaySFX2D(shotClip[Random.Range(0, shotClip.Length)]);
    }



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            _spawners = new();
            foreach (Transform child in transform)
            {
                _spawners.Add(child.GetComponent<EnemySpawner>());
            }
        }
        else
        {
            Destroy(this);
        }
    }

    public void SpawnWave(WaveSO wave)
    {
        foreach (WaveSpawnerSetup waveSpawner in wave.waveSpawnerSetups)
        {
            _spawners[waveSpawner.spawnerIndex].SpawnPattern(waveSpawner.pattern);
        }
    }
}
