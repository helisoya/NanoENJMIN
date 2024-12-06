using Unity.Mathematics.Geometry;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Represents a player's projectile
/// </summary>
public class PlayerProjectile : MonoBehaviour
{
    [Header("Infos")]
    [SerializeField] private float speed = 5;
    [SerializeField] private Renderer projectileRender;
    private ColorTarget color;

    void OnSpawn(ColorTarget color)
    {
        this.color = color;
        projectileRender.material = GameManager.instance.GetPlayerMaterial(((int)color) - 1);
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider collider)
    {
        print(collider);
    }
}
