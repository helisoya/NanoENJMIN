using UnityEngine;

public class BossDoor : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 20;
    [SerializeField] private HitFlash _hitFlash;

    private float _currentHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerProjectile"))
        {
            if (other.TryGetComponent<PlayerProjectile>(out var playerProjectile))
            {
                int damage = playerProjectile.GetDamage();
                if (damage != 0)
                {
                    TakeHit(damage);
                }
                else
                {
                    EnemyManager.instance.PlayShieldNoDmgClip();
                }

                playerProjectile.DestroyProjectile();
            }
        }
    }

    private void TakeHit(int damage)
    {
        _currentHealth -= damage;
        // Particles, flash hit animation...
        _hitFlash.HitFlashAnimation();
        
        if (_currentHealth <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        // Particles, explosion animation, etc
        foreach (var player in GameManager.instance.players)
        {
            player.AddScore(50);
        }
        gameObject.SetActive(false);
        GameManager.instance.EndGame();
    }
}
