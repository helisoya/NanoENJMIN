using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    private List<EnemySpawner> _spawners;
    
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
