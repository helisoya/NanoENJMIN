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
    private Player parent;

    /// <summary>
    /// Initialize the projectile
    /// </summary>
    /// <param name="color">The projectile's color</param>
    /// <param name="parent">The projectile's parent</param>
    public void OnSpawn(ColorTarget color, Player parent)
    {
        this.parent = parent;
        this.color = color;
        projectileRender.material = GameManager.instance.GetPlayerMaterial(color);
    }

    /// <summary>
    /// Gets the projectile's parent
    /// </summary>
    /// <returns>The parent</returns>
    public Player GetParent()
    {
        return parent;
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

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (transform.position.x >= 21.2f) Destroy(gameObject);
    }
}
