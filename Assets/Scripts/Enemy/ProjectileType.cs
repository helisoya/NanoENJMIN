using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileType", menuName = "Scriptable Objects/ProjectileType")]
public class ProjectileType : ScriptableObject
{
    public GameObject prefab;
    
    public float speed;

    public int inkRecharge;
}
