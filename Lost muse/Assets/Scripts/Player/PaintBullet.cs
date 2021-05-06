using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBullet : MonoBehaviour
{
    public Transform bulletPoint;
    public GameObject bulletPrefab;
    private Animator anim;
    [SerializeField] private float fireRate;
    private float nextFire;
    public CharacterController2D characterController;

    private void Start()
    {
        nextFire = Time.time;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(characterController.m_CanShoot == true)
        {
            if (Input.GetButtonDown("Shoot"))
            {
                Shoot();
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
        }
        
    }
}
