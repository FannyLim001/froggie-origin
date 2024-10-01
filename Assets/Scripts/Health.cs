using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health;
    public int numOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public Animator animator;
    public AudioSource hurtSound;
    public SpriteRenderer playerSprite;
    private Color originalColor;

    void Start()
    {
        originalColor = playerSprite.color;
    }

    public void TakeDamage(int damage)
    {
        StartCoroutine(FlashRed());
        hurtSound.Play();
        health -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage, remaining health: " + health);

        // Update the UI immediately after taking damage
        UpdateHearts();

        if (health <= 0)
        {
            Die();
        }
    }

    public void ResetHealth()
    {
        health = 0;
    }

    void Update()
    {
        // Keep health within the bounds of numOfHearts
        if (health > numOfHearts)
        {
            health = numOfHearts;
        }

        UpdateHearts();
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            hearts[i].enabled = i < numOfHearts;
        }
    }

    private void Die()
    {
        // Add any death-related logic here (e.g., destroy the object, play animation)
        Debug.Log(gameObject.name + " died.");
        animator.SetBool("IsDied", true);

        // Calculate the length of the death animation
        float deathAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;

        // Start coroutine to destroy the object after the animation has played
        StartCoroutine(DestroyAfterAnimation(deathAnimationLength));

        GameManager.Instance.PlayerDie();
    }

    private IEnumerator DestroyAfterAnimation(float delay)
    {
        // Wait for the duration of the death animation
        yield return new WaitForSeconds(delay);

        // Destroy the object after the animation has finished playing
        Destroy(gameObject);
    }

    private IEnumerator FlashRed()
    {
        Color flashColor = new Color(1.0f, 0.596f, 0.596f);

        // Change color to red
        playerSprite.color = flashColor;

        // Wait for a short duration
        yield return new WaitForSeconds(0.1f);

        // Revert to original color
        playerSprite.color = originalColor;
    }

}
