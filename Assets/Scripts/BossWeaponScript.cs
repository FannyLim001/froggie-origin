using UnityEngine;

public class BossWeaponScript : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public AudioSource shootClip;

    public Transform meleeAttackPoint;
    public float meleeAttackRange = 0.5f;
    public int meleeAttackDamage = 20;
    public LayerMask playerLayer;

    public float detectionRange = 10f;
    public float fireRate = 1f;

    private float nextTimeToFire = 0f;
    private bool isAlive = true;

    public Animator animator;

    void Update()
    {
        if (!isAlive) return;

        if (Time.time >= nextTimeToFire)
        {
            if (IsPlayerInMeleeRange())
            {
                animator.SetBool("IsAttacking", true);
                MeleeAttack();
            }
            else
            {
                animator.SetBool("IsAttacking", false);
                DetectAndShootTarget();
            }

            nextTimeToFire = Time.time + 1f / fireRate;
        }
    }

    void MeleeAttack()
    {
        shootClip.Play();

        Collider2D hitPlayer = Physics2D.OverlapCircle(meleeAttackPoint.position, meleeAttackRange, playerLayer);

        if (hitPlayer != null)
        {
            hitPlayer.GetComponent<Health>().TakeDamage(meleeAttackDamage);
        }
    }

    void DetectAndShootTarget()
    {
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        firePoint.localScale = new Vector3(transform.localScale.x, firePoint.localScale.y, firePoint.localScale.z);
        firePoint.right = direction;

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction, detectionRange, playerLayer);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        shootClip.Play();
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetShooterCollider(GetComponent<Collider2D>(), "Player");
    }

    bool IsPlayerInMeleeRange()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(meleeAttackPoint.position, meleeAttackRange, playerLayer);
        return hitPlayer != null;
    }

    public void Die()
    {
        isAlive = false;
    }

    private void OnDestroy()
    {
        isAlive = false;
    }
}
