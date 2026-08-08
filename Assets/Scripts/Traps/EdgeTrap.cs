using UnityEngine;

public class EdgeTrap : MonoBehaviour
{
    [SerializeField] private int damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats stats = collision.GetComponent<PlayerStats>();
        stats.DamagePlayer(damage);
    }
}
