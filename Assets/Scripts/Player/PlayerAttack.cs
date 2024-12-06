using UnityEngine;

/// <summary>
/// Handles the player's attacks
/// </summary>
public class PlayerAttack : MonoBehaviour
{
    [Header("Infos")]
    [SerializeField] private Player player;
    [SerializeField] private float fireRate = 0.3f;
    [SerializeField] private GameObject prefabProjectile;
    private float lastAttack;
    private bool canAttack;


    /// <summary>
    /// Sets the ability to fire
    /// </summary>
    /// <param name="value">Can the player fire ?</param>
    public void SetCanAttack(bool value)
    {
        canAttack = value;
    }

    void Update()
    {
        if (canAttack && Time.time - lastAttack >= fireRate)
        {
            lastAttack = Time.time;
            Instantiate(prefabProjectile, transform.position, Quaternion.identity).SendMessage("OnSpawn", player.Color);
        }
    }
}
