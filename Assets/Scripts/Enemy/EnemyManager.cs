using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [SerializeField] private List<EnemySpawner> _spawners;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void SpawnWave(WaveSO wave)
    {
        foreach (WaveSpawnerSetup waveSpawner in wave.waveSpawnerSetups)
        {
            _spawners[waveSpawner.spawnerIndex].SpawnPattern(waveSpawner.pattern);
        }
    }
}
