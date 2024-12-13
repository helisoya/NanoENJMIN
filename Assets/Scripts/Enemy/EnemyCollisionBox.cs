using UnityEngine;

public class EnemyCollisionBox : MonoBehaviour
{
    [SerializeField] private bool _isCollisionBox;

    private Enemy _enemy;

    private void Start()
    {
        _enemy = transform.parent.GetComponent<Enemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isCollisionBox && other.CompareTag("PlayerCollisionBox"))
        {
            other.attachedRigidbody.SendMessage("OnTakeDamage", 1, SendMessageOptions.DontRequireReceiver);
            _enemy.TakeHit(3, other.attachedRigidbody.GetComponent<Player>());
        }
        else if (other.CompareTag("PlayerProjectile"))
        {
            var playerProjectile = other.GetComponent<PlayerProjectile>();
            _enemy.OnPlayerProjectileHit(playerProjectile);
        }
        else if (other.CompareTag("Bound"))
        {
            Destroy(gameObject);
        }
    }
}