﻿
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {   
        //moves bullet
        rb.velocity = transform.right * Speed * testCharMovementScript.charMoveScript.GetDirection();
    }
    //chekcs for bullet hits
    void OnTriggerEnter2D( Collider2D hitInfo)
    {
        //cahnge this when we have an enemyDebug.Log(hitInfo.name);
        Destroy(gameObject);
        if (hitInfo.gameObject.CompareTag("enemy"))
        {
            Destroy(hitInfo.gameObject);
        }
    }

    // Update is called once per frame
   
}
