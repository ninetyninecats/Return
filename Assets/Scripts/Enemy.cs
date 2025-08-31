using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float leftWalking;
    [SerializeField] float rightWalking;
    BoxCollider2D collider;
    Animator animator;
    GameObject player;
    bool movingLeft;
    int health;
    public void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        health = 15;
    }
    public void Update()
    {
        if (movingLeft)
        {
            if (transform.position.x > leftWalking)
            {
                transform.position = new Vector3(transform.position.x - 3 * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else movingLeft = false;
            
        }
        else
        {
            if (transform.position.x < rightWalking)
            {
                transform.position = new Vector3(transform.position.x + 3 * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else movingLeft = true;
        }
    }
    public void TakeDamage(int damage)
    {
        if (damage >= health) Destroy(gameObject);
        health -= damage;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<PlayerStats>().TakeDamage(1);
    }
}
