using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillUnits : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health targetHealth = collision.gameObject.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(4);
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth targetHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(30);
            }
        }
    }
}
