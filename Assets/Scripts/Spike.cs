using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Touched spike");
        PlayerStats playerStats;
        if (collision.CompareTag("Player"))
        {
            playerStats = collision.GetComponent<PlayerStats>();
            playerStats.TakeDamage(2);
        }
    }
}
