using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RambutanManagerScript : MonoBehaviour
{
    public BumperScript leftBumper;
    public BumperScript rightBumper;
    private testCharMovementScript charMove;
    private Rigidbody2D rBody;
    private bool isStickingLeft;
    private bool isStickingRight;
    public int jumpMagnitude;
    private float gravityConstant;
    void Awake()
    {
        charMove = GetComponent<testCharMovementScript>();
        rBody = GetComponent<Rigidbody2D>();
        gravityConstant = rBody.gravityScale;
        isStickingLeft = false;
        isStickingRight = false;
        jumpMagnitude = 60;
        //rBody.constraints = RigidbodyConstraints2D.FreezeRotationZ;
    }
    void Launch(int dir)
    {
        rBody.constraints = RigidbodyConstraints2D.None;
        rBody.velocity = new Vector2(0, 0);
        rBody.gravityScale = 0;
        isStickingLeft = false;
        isStickingRight = false;
        
        //Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.PI/3;
        isStickingLeft = false;
        isStickingRight = false;
        
        rBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        rBody.velocity += new Vector2(dir * jumpMagnitude * Mathf.Cos(angle), jumpMagnitude * Mathf.Sin(angle));
        rBody.gravityScale = gravityConstant;
    }
    // Update is called once per frame
    void Update()
    {
       if(!charMove.isGrounded)
        {
            if (Input.GetKey(KeyCode.A) && leftBumper.isTouchingWall && !isStickingLeft)
            {
                
                isStickingLeft = true;
            }
            if (Input.GetKey(KeyCode.D) && rightBumper.isTouchingWall && !isStickingRight)
            {
                
                isStickingRight = true;
            }
        }

        if (!leftBumper.isTouchingWall || !Input.GetKey(KeyCode.A))
        {
            rBody.constraints = RigidbodyConstraints2D.None;
            rBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            isStickingLeft = false;
        }
        if (!rightBumper.isTouchingWall || !Input.GetKey(KeyCode.D))
        {
            rBody.constraints = RigidbodyConstraints2D.None;
            rBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            isStickingRight = false;
        }
        
        if(isStickingLeft || isStickingRight)
        {
            rBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
            rBody.velocity = new Vector2(0, 0);
        }

        if(Input.GetKeyUp(KeyCode.W) && !charMove.isGrounded && (isStickingLeft || isStickingRight))
        {
            if (leftBumper.isTouchingWall)
            {
                Launch(1);
            }
            else if (rightBumper.isTouchingWall)
            {
                Launch(-1);
            }
        }
        
    }
}

//Hold Down side TO stay Stuck.
//V makes char do it a short, predetermined hop in a given direction.