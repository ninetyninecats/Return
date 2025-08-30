using UnityEngine;

public class Enemy : MonoBehaviour
{
    float attackCooldown;
    float cooldownTimer;
    BoxCollider2D collider;
    int health;
    public void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        health = 15;
    }
    public void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= attackCooldown && CanSeePlayer())
        {

        }
    }
    private bool CanSeePlayer()
    {
        RaycastHit2D raycast = Physics2D.BoxCast(collider.bounds.center + transform.right * transform.localScale.x, Vector2.one, 0, Vector2.left, 0, 6);
        return raycast.collider != null;
    }
    public void TakeDamage(int damage)
    {
        if (damage >= health) Destroy(gameObject);
        health -= damage;
    }
}
