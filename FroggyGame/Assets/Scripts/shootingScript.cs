
using UnityEngine;

public class shootingScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform firePoint;
    public GameObject bulletPrefab;

    // Update is called once per frame
    void Update()
    {   
        //shoots ifs right mouse is pressed
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        //createsa bullet from the prefab
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
