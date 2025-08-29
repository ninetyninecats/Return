using UnityEngine;

public class Enemy : MonoBehaviour
{
    float attackCooldown;
    float cooldownTimer;
    BoxCollider2D collider;
    public void Awake() {
        collider = GetComponent<BoxCollider2D>();
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
        RaycastHit2D raycast = Physics2D.BoxCast(collider.bounds.center + transform.right * transform.localScale.x, Vector2.one , 0, Vector2.left, 0, 6);    
        return raycast.collider != null;
    }
}
