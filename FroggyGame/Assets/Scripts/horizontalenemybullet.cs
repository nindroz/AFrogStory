using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class horizontalenemybullet : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform firePoint;
    public GameObject bulletPrefab;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, Vector2.right, 2f); //raycast shooting from gameobject right
        if (hitInfo.collider == true)
        {
            Shoot();
        }

        void Shoot()
        {
            //createsa bullet from the prefab
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}
