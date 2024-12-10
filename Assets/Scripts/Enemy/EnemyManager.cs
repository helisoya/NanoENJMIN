using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [SerializeField] private List<EnemySpawner> _spawners;

    //TEST
    [SerializeField] private List<WaveSO> _waves;
    [SerializeField] private int _waveIndex;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Update()
    {
        //TEST
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnWave(_waves[_waveIndex]);
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
