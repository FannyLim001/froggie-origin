using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMushroom : MonoBehaviour
{
    public Animator animator;
    public AudioSource jumpSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jumpSFX.Play();
            animator.SetTrigger("Jumped");

            // Apply a constant force, boosting the jump
            Rigidbody2D rb = collision.attachedRigidbody;
            rb.AddForce(new Vector2(0f, 30f), ForceMode2D.Impulse); // Adjust the 10f for desired jump strength
        }
    }

}
