using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public enum FireMode
{
    Single,
    Burst,
    Homing
}
public enum TargetingMode
{
    None,
    Locked,
    PredictedLocked
}
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
    [HideInInspector] public FireMode fireMode;
    [HideInInspector] public ProjectileTypeSO projectileTypeSo;
    [FormerlySerializedAs("targetingType")] [HideInInspector] public TargetingMode targetingMode;
    [HideInInspector] public float fireAngleRange;
    [HideInInspector] public float fireRate;
    
    [HideInInspector] public int nbBurstProjectiles;
    [HideInInspector] public float burstProjectileAngleSpacing;
}
