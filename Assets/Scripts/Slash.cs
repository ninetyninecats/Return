using UnityEngine;

public class Slash : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;

    void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 9) return;
        collision.gameObject.GetComponent<Enemy>().TakeDamage(5);
    }
}
