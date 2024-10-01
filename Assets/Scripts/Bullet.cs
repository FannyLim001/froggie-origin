using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public GameObject impactEffect;
    public float lifetime = 2f; // Time before the bullet is destroyed if it doesn't hit anything
    public float impactEffectLifetime = 3f; // Time before the impact effect is destroyed
    public float maxRange = 5f; // Maximum distance the bullet can travel before being destroyed
    private Vector2 startPosition; // The bullet's initial position
    private Collider2D shooterCollider;
    private string targetTag;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
        startPosition = rb.position;
        Destroy(gameObject, lifetime); // Destroy bullet after a certain lifetime
    }

    public void SetShooterCollider(Collider2D collider, string targetTag)
    {
        shooterCollider = collider;
        this.targetTag = targetTag;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), shooterCollider);
    }

    private void Update()
    {
        // Check if the bullet has traveled beyond its max range
        float distanceTraveled = Vector2.Distance(startPosition, rb.position);
        if (distanceTraveled >= maxRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore collision with the shooter or other objects that aren't the intended target
        if (collision == shooterCollider || !collision.CompareTag(targetTag))
        {
            return;
        }

        Debug.Log(collision.name);

        // Damage player or enemy based on tag
        if (collision.CompareTag("Player"))
        {
            Health targetHealth = collision.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(1);
            }
        }
        else if (collision.CompareTag("Enemy"))
        {
            EnemyHealth targetHealth = collision.GetComponent<EnemyHealth>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(10);
            }
        }

        // Create impact effect and destroy the bullet
        GameObject impact = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(impact, impactEffectLifetime);

        Destroy(gameObject);
    }
}
