using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private void CreateEnemy(EnemyType enemyType, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        GameObject spawnedEnemy = Instantiate(enemyType.prefab, spawnPosition, spawnRotation);
        spawnedEnemy.AddComponent<Enemy>().Initialize(enemyType);
    }
}
