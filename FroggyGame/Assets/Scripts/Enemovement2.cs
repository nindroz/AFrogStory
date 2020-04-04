using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemovement2 : MonoBehaviour
{
    public float speed = 5f;
    public float groundCheckDistance;
    public float wallCheckDistance;
    private int layer;

    private bool movingLeft;
    private bool wallDetected;


    public Transform groundCheck;
    public Transform wallCheck;
    // public GameObject bulletPrefab;
    // Update is called once per frame
    private void Start()
    {
        movingLeft = true;
        layer = LayerMask.GetMask("Terrain");
    }
    private void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        RaycastHit2D groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, layer);
        RaycastHit2D wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.left, wallCheckDistance, layer);
        if (groundDetected.collider == false || wallDetected.collider == true)
        {
            if (movingLeft == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingLeft = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingLeft = true;

            }

            



            /*if (wallDetected.collider == true)
                {
                    render.flipX = true;
                    transform.Translate(Vector2.right * speed * Time.deltaTime);
                    transform.eulerAngles = new Vector3(0, -180, 0);
                }
                else
                {
                    render.flipX = false;
                    transform.Translate(Vector2.left * speed * Time.deltaTime);
                }*/
        }




        /*
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        //RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, Vector2.left, distance);
        if (groundInfo.collider == false)
        {
            if (movingLeft == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingLeft = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingLeft = true;
            }
        }
        /*if (hitInfo.collider == true)
        {
            Shoot();
        }

        void Shoot()
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }*/
    }


}
