using UnityEngine;

public class PlayerAbsorptionBox : MonoBehaviour
{
    [SerializeField] private Player player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyProjectile"))
        {
            var enemyProjectile = other.GetComponent<EnemyProjectile>();
            var inkRecharge = enemyProjectile.GetInkToRecharge(player.Color);
            if (inkRecharge > 0f)
            {
                player.Recharge(inkRecharge);
                other.GetComponent<EnemyProjectile>().DoAbsorption(transform);
            }
        }
    }
}
