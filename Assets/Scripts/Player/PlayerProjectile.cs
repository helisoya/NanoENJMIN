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
    [SerializeField] private int damage = 1;

    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private float destroyAfter;
    private bool isDestroyed = false;
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
        this.isDestroyed = false;
        animator.runtimeAnimatorController = GameManager.instance.GetPlayerBulletController(color);
    }

    /// <summary>
    /// Gets the projectile's parent
    /// </summary>
    /// <returns>The parent</returns>
    public Player GetParent()
    {
        return parent;
    }

    /// <summary>
    /// Destroyes the projectile
    /// </summary>
    public void DestroyProjectile()
    {
        animator.SetTrigger("Destroy");
        isDestroyed = true;
        Destroy(gameObject, destroyAfter);
    }

    public int GetDamage(bool hasShield, ColorTarget targetColour)
    {
        if (isDestroyed) return 0;

        if (hasShield)
        {
            if (targetColour != color)
                return 0;
        }

        return damage;
    }

    public int GetDamage()
    {
        if (isDestroyed) return 0;
        return damage;
    }

    void Update()
    {
        if (!GameManager.instance.InGame)
        {
            return;
        }
        if (isDestroyed) return;

        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (transform.position.x >= 21.2f) Destroy(gameObject);
    }
}
