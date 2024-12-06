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
    private bool pressedFire;


    /// <summary>
    /// Sets the ability to fire
    /// </summary>
    /// <param name="value">Can the player fire ?</param>
    public void SetCanAttack(bool value)
    {
        pressedFire = value;
    }

    void Update()
    {
        if (pressedFire && player.HasMana && Time.time - lastAttack >= fireRate)
        {
            lastAttack = Time.time;
            player.AddMana(-1);
            Instantiate(prefabProjectile, transform.position, Quaternion.identity).SendMessage("OnSpawn", player.Color);
        }
    }
}
