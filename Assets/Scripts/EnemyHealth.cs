using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 30f;
    private float currentHealth;

    public Animator animator;
    public AudioSource hurtSound;
    public SpriteRenderer enemySprite;
    private Color originalColor;

    private Weapon weapon;
    private bool isDead = false; // Ensure Die() is only called once
    public bool isBoss = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        originalColor = enemySprite.color; // Store the original color
        weapon = GetComponent<Weapon>();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return; // Prevent further damage if already dead

        // Apply red color effect
        StartCoroutine(FlashRed());
        hurtSound.Play();
        currentHealth -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage, remaining health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false; // Allow the enemy to take damage again
    }

    private void Die()
    {
        if (isDead) return; // Ensure Die() only runs once

        isDead = true; // Mark as dead
        Debug.Log(gameObject.name + " died.");
        animator.SetBool("IsDied", true);

        // Disable the collider so the player can pass through
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false; // Disable the collider
        }

        // Optional: Disable physics interaction (if using Rigidbody2D)
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero; // Stop any movement
            rb.isKinematic = true; // Prevent further physics interactions
        }

        // Check and call weapon's death function
        if (weapon != null)
        {
            weapon.Die();
        }

        // Notify GameManager
        GameManager.Instance.OnEnemyDefeated();

        if (isBoss)
        {
            GameManager.Instance.OnBossDefeated();
        }

        // Calculate the length of the death animation
        float deathAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;

        // Start coroutine to destroy the object after the animation has played
        StartCoroutine(DestroyAfterAnimation(deathAnimationLength));
    }

    private IEnumerator DestroyAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private IEnumerator FlashRed()
    {
        // Create a less saturated red color
        Color flashColor = new Color(1.0f, 0.596f, 0.596f); // Adjust these values to achieve the desired red tint

        enemySprite.color = flashColor;

        // Wait for a short duration
        yield return new WaitForSeconds(0.1f);

        // Revert to original color
        enemySprite.color = originalColor;
    }
}
