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

        if (transform.position.x >= 30) Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {
        print(collider);
    }

    public int GetDamage(bool hasShield, ColorTarget targetColour)
    {
        if (hasShield)
        {
            if (targetColour != color)
                return 0;
        }

        return 1;
    }
}
