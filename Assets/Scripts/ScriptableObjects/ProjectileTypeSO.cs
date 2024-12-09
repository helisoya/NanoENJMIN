using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileType", menuName = "Scriptable Objects/ProjectileType")]
public class ProjectileTypeSO : ScriptableObject
{
    public GameObject prefab;
    public List<Material> colourMaterials;
    
    public float speed;

    public float inkRecharge;
}
