using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [SerializeField] private List<EnemyTypeSO> _enemyTypes;
    
    [SerializeField] private List<GameObject> _enemySpawnPoints;

    [SerializeField] private float _spawnRate;

    private float _spawnTimer;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        _spawnTimer = _spawnRate;
    }

    private void Update()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        print("doing spawning");
        GameObject spawnPoint = _enemySpawnPoints[Random.Range(0, _enemySpawnPoints.Count)];
        EnemyTypeSO spawnEnemyTypeSo = _enemyTypes[Random.Range(0, _enemyTypes.Count)];
        
        CreateEnemy(spawnEnemyTypeSo, spawnPoint.transform.position, spawnPoint.transform.rotation);
        
        _spawnTimer = _spawnRate;
    }

    private void CreateEnemy(EnemyTypeSO enemyTypeSo, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        GameObject spawnedEnemy = Instantiate(enemyTypeSo.prefab, spawnPosition, spawnRotation);
        spawnedEnemy.AddComponent<Enemy>().Initialize(enemyTypeSo);
    }

    public void SpawnWave(WaveSO wave)
    {
        
    }
}
