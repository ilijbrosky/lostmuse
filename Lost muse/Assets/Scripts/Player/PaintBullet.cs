using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBullet : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform bulletPoint;
    public GameObject bulletPrefab;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Shoot"))
        {
            Shoot();
        }
    }
    void Shoot()
    {
        Instantiate(bulletPrefab, bulletPoint.position, bulletPoint.rotation);

    }
}
