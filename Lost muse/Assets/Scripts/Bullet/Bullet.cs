using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float speed;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6) // GroundCheck layer
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 9) // Enemy layer
        {
            boxCollider.isTrigger = true;
        }
        if(collision.gameObject.layer == 11) // Layer named "Walls"
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9) // Enemy layer
        {
            boxCollider.isTrigger = false;
        }
    }
}
