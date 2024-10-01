using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float detectionRange = 10f;
    public float stopDistance = 2f; // Distance at which the enemy stops chasing the player
    public float jumpForce = 10f; // Adjusted for a stronger jump
    public float groundCheckRadius = 0.2f; // Adjust radius as needed
    public float edgeCheckDistance = 0.5f; // Distance to check for edges
    public Transform groundCheck;
    public Transform edgeCheck;
    public LayerMask groundLayer;
    private Transform player;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isNearEdge;

    public Animator animator;

    public float patrolDistance = 2f; // Distance to patrol from the starting point
    private float patrolLeftLimit;
    private float patrolRightLimit;
    private bool movingRight = true; // Direction of patrol
    private bool isChasing = false; // Indicates whether the enemy is chasing the player

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Set patrol limits based on the starting position
        patrolLeftLimit = transform.position.x - patrolDistance;
        patrolRightLimit = transform.position.x + patrolDistance;
    }

    void Update()
    {
        CheckGround();
        CheckEdge();

        float distanceToPlayer = player != null ? Vector2.Distance(transform.position, player.position) : Mathf.Infinity;

        // Check if the enemy should chase the player or stop moving due to proximity
        if (distanceToPlayer < detectionRange && distanceToPlayer > stopDistance)
        {
            isChasing = true;
        }
        else if (distanceToPlayer <= stopDistance)
        {
            isChasing = false; // Stop chasing if the player is within the stop distance
            rb.velocity = new Vector2(0, rb.velocity.y); // Stop the enemy's movement
            FacePlayer(); // Keep facing the player
            return; // Prevent further movement logic
        }
        else
        {
            isChasing = false;
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void FacePlayer()
    {
        if (player == null) return;

        float direction = player.position.x - transform.position.x;

        // Flip the enemy to face the player
        if (direction > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void CheckEdge()
    {
        isNearEdge = !Physics2D.Raycast(edgeCheck.position, Vector2.down, edgeCheckDistance, groundLayer);
    }

    void ChasePlayer()
    {
        if (player == null) return;

        float direction = player.position.x - transform.position.x;

        // Check if the enemy is actually moving before flipping
        if (Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            if (direction > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);
        }

        // Move horizontally towards the player
        rb.velocity = new Vector2(Mathf.Sign(direction) * moveSpeed, rb.velocity.y);

        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

        // Jump if the enemy is near the edge or if there's no ground ahead
        if (isNearEdge && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }


    void Patrol()
    {
        // Determine the direction of patrol
        if (movingRight)
        {
            // If the enemy is moving right but has reached the patrol limit, change direction
            if (transform.position.x >= patrolRightLimit)
            {
                movingRight = false;
            }
            else
            {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else
        {
            // If the enemy is moving left but has reached the patrol limit, change direction
            if (transform.position.x <= patrolLeftLimit)
            {
                movingRight = true;
            }
            else
            {
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

        // Jump if the enemy is near the edge or if there's no ground ahead
        if (isNearEdge && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}
