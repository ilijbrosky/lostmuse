using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private int damage = 40;
    [Range(0, 10)]
    [SerializeField] private float destroyTime;
    public Rigidbody2D rb;
    public GameObject impactEffect;
    public BoxCollider2D boxCollider;

    [System.Obsolete]
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        DestroyObject(gameObject, destroyTime);
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * speed * Time.fixedDeltaTime);
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
        else if (collision.gameObject.tag == "Enemy")
        {
            boxCollider.isTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            boxCollider.isTrigger = false;
        }
    }
}
