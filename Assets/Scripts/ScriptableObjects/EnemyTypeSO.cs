using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "EnemyType", menuName = "Scriptable Objects/EnemyType")]
public class EnemyTypeSO : ScriptableObject
{
    public GameObject prefab;
    public ColorTarget colour;
    public float speed;
    public int score;
    public int lifePoints;

    [HideInInspector]public bool hasShield;
    [HideInInspector]public Material shieldMaterial;
    [HideInInspector]public int shieldLifePoints;

    [HideInInspector] public bool canFire;
    [FormerlySerializedAs("projectileType")] [HideInInspector]public ProjectileTypeSO projectileTypeSo;
    [HideInInspector]public float fireRate;
}
