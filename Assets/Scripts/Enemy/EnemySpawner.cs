using System;
using System.Collections;
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

    public void SpawnPattern(EnemyPatternSO pattern)
    {
        StartCoroutine(EnemySpawnCoroutine(pattern));
    }

    private IEnumerator EnemySpawnCoroutine(EnemyPatternSO pattern)
    {
        int enemiesSpawned = 0;
        while (enemiesSpawned < pattern.enemies.Count)
        {
            CreateEnemy(pattern.enemies[enemiesSpawned], _spline.Spline.EvaluatePosition(0f), transform.rotation);
            enemiesSpawned++;
            yield return new WaitForSeconds(pattern.spawnRate);
        }

        yield return null;
    }
    
    private void CreateEnemy(EnemyTypeSO enemyTypeSo, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        GameObject spawnedEnemy = Instantiate(enemyTypeSo.prefab, spawnPosition, spawnRotation);
        spawnedEnemy.AddComponent<Enemy>().Initialize(enemyTypeSo);
    }
}
