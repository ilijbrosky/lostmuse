using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float speed = 20f;
    [SerializeField] private int damage = 40;
    [Range(0, 10)]
    [SerializeField] private float destroyTime;
    public Rigidbody2D rb;
    public GameObject impactEffect;

    [System.Obsolete]
    void Start()
    {
        rb.velocity = transform.right * speed;
        DestroyObject(gameObject, destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "GroundCheck")
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if ((collision.gameObject.tag == "Enemy"))
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
