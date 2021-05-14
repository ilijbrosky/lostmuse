using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageTaker : MonoBehaviour
{



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            //health = -30;

        }
    }


}
