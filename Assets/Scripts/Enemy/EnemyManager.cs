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
        AudioManager.instance.PlaySFX2D(deathclips[Random.Range(0, deathclips.Length)], 1);
    }

    public void PlayHitClip()
    {
        AudioManager.instance.PlaySFX2D(hitClips[Random.Range(0, hitClips.Length)], 2);
    }


    public void PlayShieldBreakClip()
    {
        AudioManager.instance.PlaySFX2D(shieldBreakClip[Random.Range(0, shieldBreakClip.Length)], 3);
    }


    public void PlayShieldHitClip()
    {
        AudioManager.instance.PlaySFX2D(shieldHitClip[Random.Range(0, shieldHitClip.Length)], 4);
    }


    public void PlayShieldNoDmgClip()
    {
        AudioManager.instance.PlaySFX2D(shieldNoDmgClip[Random.Range(0, shieldNoDmgClip.Length)], 5);
    }

    public void PlayShotClip()
    {
        // AudioManager.instance.PlaySFX2D(shotClip[Random.Range(0, shotClip.Length)], 2);
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

    public void SpawnWave(List<SpawnSetup> spawnSetups)
    {
        foreach (var spawnSetup in spawnSetups)
        {
            _spawners[spawnSetup.spawnerIndex].SpawnEnemies(spawnSetup.enemies, spawnSetup.spawnRate);
        }
    }
}
