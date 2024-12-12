using UnityEngine;

/// <summary>
/// Handles the player's attacks
/// </summary>
public class PlayerAttack : MonoBehaviour
{
    [Header("Infos")]
    [SerializeField] private Player player;
    [SerializeField] private float fireRate = 0.3f;
    [SerializeField] private PlayerProjectile prefabProjectile;
    [SerializeField] private AudioClip[] fireClips;
    [SerializeField] private float manaCost = 1f;
    [SerializeField] private Transform shootParticlesPos;

    private ParticleSystem _shootParticles;
    private float lastAttack;
    private bool pressedFire;

    private void Start()
    {

        _shootParticles = Instantiate(GameManager.instance.GetShootParticles(player.Color), shootParticlesPos.position, Quaternion.identity, shootParticlesPos);
    }


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
        if (player.Alive && pressedFire && (GameManager.instance.cheatHasInfiniteAmmo || player.Mana >= manaCost) && Time.time - lastAttack >= fireRate)
        {
            AudioManager.instance.PlaySFX2D(fireClips[Random.Range(0, fireClips.Length)]);
            lastAttack = Time.time;
            
            if (!GameManager.instance.cheatHasInfiniteAmmo)
            {
                player.AddMana(-manaCost);
            }
            Instantiate(prefabProjectile, shootParticlesPos.position, Quaternion.identity).OnSpawn(player.Color, player);
            _shootParticles.Play();
            player.SetWeaponAnimationTrigger("Fire");
            
        }
        else if (player.Alive && pressedFire && player.Mana < manaCost && Time.time - lastAttack >= fireRate)
        {
            player.SetWeaponAnimationTrigger("FireEmpty");
        }
    }
}
