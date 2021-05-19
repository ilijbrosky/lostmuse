using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBullet : MonoBehaviour
{
    public Transform bulletPoint;
    public GameObject bulletPrefab;
    public CharacterController2D characterController;
    private Animator anim;
    
    private float nextFire;
    [SerializeField] private float fireRate;
    [SerializeField] private int fireCount;



    private void Start()
    {
        nextFire = Time.time;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (characterController.m_CanShoot == true && characterController.m_CanMove)
        {
            if (Input.GetButtonDown("Shoot"))
            {

                for(int i = 0; i < fireCount; i++)
                {
                    Shoot();
                }

            }
        }
        

    }
    void Shoot()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(bulletPrefab, bulletPoint.position, bulletPoint.rotation);
            anim.SetTrigger("Attack");
            fireCount--;
        }
        
    }
}
