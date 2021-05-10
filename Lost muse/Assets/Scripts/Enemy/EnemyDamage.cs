using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public HealthSystem playerHealth;
    private bool canTakeDamage = false;
    public void DamagePlayer()
    {
        if (canTakeDamage)
        {
            playerHealth.TakeDamage();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canTakeDamage = true;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canTakeDamage = false;
        }
    }

}
