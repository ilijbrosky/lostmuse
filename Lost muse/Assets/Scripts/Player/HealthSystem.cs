using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public GameObject[] hearts;
    [SerializeField] private int life;
    //[SerializeField] private int healthCount;
 

    // Update is called once per frame
    void Update()
    {
        if (life < 60)
        {
            hearts[0].SetActive(false);
        }
        if (life < 30)
        {
            hearts[1].SetActive(false);
        }
        if (life < 0)
        {
            hearts[2].SetActive(false);
        }
    }

    public void TakeDamage()
    {
        life -= 10 ;
    }
}
