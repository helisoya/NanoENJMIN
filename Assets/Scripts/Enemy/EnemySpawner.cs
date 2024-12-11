using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineContainer))]
public class EnemySpawner : MonoBehaviour
{
    private SplineContainer _spline;

    private void Awake()
    {
        _spline = GetComponent<SplineContainer>();
    }

    public void SpawnEnemies(List<EnemyTypeSO> enemies, float spawnRate)
    {
        StartCoroutine(EnemySpawnCoroutine(enemies, spawnRate));
    }

    private IEnumerator EnemySpawnCoroutine(List<EnemyTypeSO> enemies, float spawnRate)
    {
        int enemiesSpawned = 0;
        while (enemiesSpawned < enemies.Count)
        {
            CreateEnemy(enemies[enemiesSpawned], (Vector3) _spline.Spline.EvaluatePosition(0f) + transform.position, Quaternion.LookRotation(-transform.right));
            enemiesSpawned++;
            yield return new WaitForSeconds(spawnRate);
        }

        yield return null;
    }
    
    private void CreateEnemy(EnemyTypeSO enemyTypeSo, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        GameObject spawnedEnemy = Instantiate(enemyTypeSo.prefab, spawnPosition, spawnRotation);
        spawnedEnemy.AddComponent<Enemy>().Initialize(enemyTypeSo, _spline, transform.position);
    }
}
