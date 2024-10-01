using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public AudioSource shootClip;

    public float detectionRange = 10f; // Range at which the enemy can detect the player
    public LayerMask targetLayer; // Layer to identify the player or target
    public string targetTag = "Player"; // Tag for the target (can be set to "Player" or any other target)
    public float fireRate = 1f; // Time in seconds between shots

    private float nextTimeToFire = 0f;
    private bool isAlive = true;

    // Update is called once per frame
    void Update()
    {
        if (CompareTag("Player"))
        {
            // Player shoots when Fire1 is pressed
            if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
            {
                Shoot();
                nextTimeToFire = Time.time + 1f / fireRate; // Set the cooldown for the next shot
            }
        }
        else if (CompareTag("Enemy"))
        {
            if (!isAlive) return;

            if (Time.time >= nextTimeToFire)
            {
                DetectAndShootTarget();
                nextTimeToFire = Time.time + 1f / fireRate;
            }
        }
    }

    void Shoot()
    {
        shootClip.Play();
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetShooterCollider(GetComponent<Collider2D>(), targetTag);
    }

    void DetectAndShootTarget()
    {
        // Determine the direction the enemy is facing
        Vector2 direction =  transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        firePoint.localScale = new Vector3(transform.localScale.x, firePoint.localScale.y, firePoint.localScale.z);
        firePoint.right = direction;

        // Perform a raycast to detect the target (e.g., the player)
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction, detectionRange, targetLayer);

        if (hit.collider != null && hit.collider.CompareTag(targetTag))
        {
            Shoot();
        }
    }

    public void Die()
    {
        isAlive = false; // Mark the enemy as dead
    }

    private void OnDestroy()
    {
        isAlive = false;
    }
}
