using UnityEngine;


/// <summary>
/// Represents a player trigger
/// </summary>
public class PlayerTriggerBox : MonoBehaviour
{
    [SerializeField] private bool handlesAbsortion = false;
    [SerializeField] private Player player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyProjectile"))
        {
            EnemyProjectile enemyProjectile = other.gameObject.GetComponent<EnemyProjectile>();

            float inkRecharge = enemyProjectile.GetInkToRecharge(player.Color);
            if (inkRecharge == 0f && !handlesAbsortion)
            {
                player.OnTakeDamage(1);
                Destroy(other.gameObject);
            }
            else if (inkRecharge > 0f && handlesAbsortion)
            {
                player.Recharge(inkRecharge);
                Destroy(other.gameObject);
            }
        }
    }
}
