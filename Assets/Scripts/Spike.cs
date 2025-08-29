using UnityEngine;

public class Spike : MonoBehaviour
{
    private static bool triggerLock;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        triggerLock = true;
        Debug.Log("Touched spike");
        PlayerStats playerStats;
        if (collision.CompareTag("Player"))
        {
            playerStats = collision.GetComponent<PlayerStats>();
            playerStats.TakeDamage(2);
        }
        triggerLock = false;
    }
}
