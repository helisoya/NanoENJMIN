using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyPattern", menuName = "Scriptable Objects/EnemyPattern")]
public class EnemyPatternSO : ScriptableObject
{
    public List<EnemyTypeSO> enemies;
    public float spawnRate;
}
