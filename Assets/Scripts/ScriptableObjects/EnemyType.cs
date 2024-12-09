using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType", menuName = "Scriptable Objects/EnemyType")]
public class EnemyType : ScriptableObject
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
    [HideInInspector]public ProjectileType projectileType;
    [HideInInspector]public float fireRate;
}
