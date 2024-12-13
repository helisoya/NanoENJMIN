using UnityEngine;

public class PlayerCollisionBox : MonoBehaviour
{
    [SerializeField] private Player player;
    
    public void TriggerEnter(GameObject other)
    {
        if (other.CompareTag("EnemyProjectile"))
        {
            var enemyProjectile = other.GetComponent<EnemyProjectile>();
            var inkRecharge = enemyProjectile.GetInkToRecharge(player.Color);
            if (inkRecharge == 0f)
            {
                player.OnTakeDamage(1);
                Destroy(other.gameObject);
            }
        }
    }
}
