using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [SerializeField] private List<EnemyType> _enemyTypes;

    [SerializeField] private List<GameObject> _enemySpawnPoints;

    [SerializeField] private float _spawnRate;

    private float _spawnTimer;

    public Action<int> onChangeWave;

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
        if (!GameManager.instance.InGame) return;

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
        EnemyType spawnEnemyType = _enemyTypes[Random.Range(0, _enemyTypes.Count)];

        CreateEnemy(spawnEnemyType, spawnPoint.transform.position, spawnPoint.transform.rotation);

        _spawnTimer = _spawnRate;
    }

    private void CreateEnemy(EnemyType enemyType, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        GameObject spawnedEnemy = Instantiate(enemyType.prefab, spawnPosition, spawnRotation);
        spawnedEnemy.AddComponent<Enemy>().Initialize(enemyType);
    }

    public void ChangeWave(int waveIndex)
    {
        onChangeWave.Invoke(waveIndex);
    }
}
