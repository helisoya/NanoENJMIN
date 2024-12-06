using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType", menuName = "Scriptable Objects/EnemyType")]
public class EnemyType : ScriptableObject
{
    public GameObject prefab;
    public List<Material> colourMaterials;
    public float speed;
    public int score;
    public int lifePoints;

    [HideInInspector]public bool hasShield;
    [HideInInspector]public ColorTarget shieldColour;
    [HideInInspector]public int shieldLifePoints;
    
    [HideInInspector]public ProjectileType projectileType;
    [HideInInspector] public ColorTarget projectileColour;
    [HideInInspector]public float fireRate;
}
