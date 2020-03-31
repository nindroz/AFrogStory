using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class horizontalenemy : MonoBehaviour
{
    // i have no idea whats going on i just following tutorial :(
    public float speed;
    public float distance = 2f;

    private bool movingRight = true;

    public Transform groundDetection;


    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime); // move ground detection gameobject to the right
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance); //raycast shooting from gameobject down
        if (groundInfo.collider == false) // if ray hasn't collided w anything 
        {
            if (movingRight == true) 
            {
                transform.eulerAngles = new Vector3(0, -180, 0); //turn left
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0); //if moving left and ray doesn't detect anything, turn right
                movingRight = true;
            }

        }
    }
}
