using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testCharMovementScript : MonoBehaviour
{
    //Note - this movement script is mass independant
    //Singleton 
    public static testCharMovementScript charMoveScript;
    public static BoxCollider2D charCollider;
    private void Awake()
    {
        charMoveScript = this;
    }

    //Components
    private Rigidbody2D charRb;
    //input
    private static float xInput;
    //Horizontal Movement vars
    public float moveVelocity = 20;//Maximum horizontal velocity
    private float moveForceGround = 130f;
    private float moveForceAir = 70f;

    //direction for other scripts
    private int direction = 0;

    //sets previous is grounded
    public static bool prevGrounded = true;
    public static bool methodGetGrounded;

    //checks for jump
    public static bool isJump;

    //Jumping vars
    public float jumpChargeTime;
    public float jumpMaxVel;
    public float jumpMinVel;
    private float jumpTimer = 0f;
    public Image jumpBar;

    //Timers

    private float jumpHoldIgnoreTime = 0.2f;//Ignores holding down spacebar for buffer time
    private float jumpIgnoreGroundedTime = 0.1f;//Buffer time to prevent drag from applying on initial jump
    private float jumpIgnoreGroundedTimer = 0f;

    //character states
    public static bool isGrounded = false;
    private bool horizontalMovementActive = true;
    private bool ignoreHorizontalDrag = false;
    void Start()
    {
        //Gets components
        charRb = gameObject.GetComponent<Rigidbody2D>();
        charCollider = gameObject.GetComponent<BoxCollider2D>();
        
        charRb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    //input
    void Update()
    {
        jumpIgnoreGroundedTimer -= Time.deltaTime;
        
        xInput = Input.GetAxisRaw("Horizontal");
        //Charging jump
        if (Input.GetKey(KeyCode.Space) && isGrounded && !TongueScript.charTongueScript.GetTongueOut())
        {
            
            jumpTimer += Time.deltaTime;
            if (jumpTimer > jumpHoldIgnoreTime)
            {
                //Updates jump bar
                jumpBar.transform.localScale = new Vector3(Mathf.Min((jumpTimer-jumpHoldIgnoreTime) / jumpChargeTime, 1), 1, 0);
                //locks horizontal movement
                horizontalMovementActive = false;
            }
           
        }
        //Release
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (isGrounded && !TongueScript.charTongueScript.GetTongueOut())
            {

                ParticleManager.particleManager.PlayJumpEffect();
                //Hold jump
                if (jumpTimer > jumpHoldIgnoreTime)
                {
                    isJump = true;
                    //Ignores drag for charged jumps
                    ignoreHorizontalDrag = true;
                    //Gets magnitude of jump and direction of jump
                    float jumpRatio = Mathf.Min((jumpTimer - jumpHoldIgnoreTime) / jumpChargeTime, 1);
                    float jumpMagnitude = Mathf.Lerp(jumpMinVel, jumpMaxVel, jumpRatio);
                    Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x);
                    angle = Mathf.PI / 2;
                    charRb.velocity = new Vector2(jumpMagnitude * Mathf.Cos(angle),jumpMagnitude* Mathf.Sin(angle));
                    //Plays sound
                    AudioManager.audioManager.PlayJumpSound();
                }
                //Normal jump
                else
                {
                    isJump = true;
                    charRb.velocity = new Vector2(charRb.velocity.x, jumpMinVel);
                }
                jumpIgnoreGroundedTimer = jumpIgnoreGroundedTime;
            }

            //Resets jumpbar and horizontalMovementActive
            isJump = true;
            jumpTimer = 0;
            jumpBar.transform.localScale = new Vector3(0, 1, 0);
            horizontalMovementActive = true;
        }
    }

    private void FixedUpdate()
    {
        //prevGrounded = isGrounded;
        isGrounded = CheckForGrounded();
        methodGetGrounded = isGrounded;
        //Prevents being checked as grounded immediately after jumping
        if (jumpIgnoreGroundedTimer > 0)
            isGrounded = false;
        //Drag always reenabled when grounded
        if (ignoreHorizontalDrag && isGrounded)
            ignoreHorizontalDrag = false;

        if ((xInput == 0 || !horizontalMovementActive) && !ignoreHorizontalDrag)
        {
            //Velocity drag when on ground
            if (isGrounded)
            {
                charRb.velocity = new Vector2(charRb.velocity.x / 1.47f, charRb.velocity.y);
            }
            //Velocity drag in air
            else
            {
                charRb.velocity = new Vector2(charRb.velocity.x / 1.1f, charRb.velocity.y);
            }
        }
        //Applies horizontal movement
        if (horizontalMovementActive)
        {
            if(xInput != 0)
                direction = (int)xInput;
            //More force while grounded
            if(isGrounded)
            {
                charRb.AddForce(Vector2.right * xInput * moveForceGround * charRb.mass);
                //Clamps ground speed to max velocity
                charRb.velocity = new Vector2(Mathf.Clamp(charRb.velocity.x, -moveVelocity, moveVelocity), charRb.velocity.y+0.1f);
            }
            //less in air
            else
            {
                //Clamps air speed to max velocity
                if(charRb.velocity.x * xInput < moveVelocity)
                    charRb.AddForce(Vector2.right * xInput * moveForceAir * charRb.mass);
            }
        }
    }
/*
    private void OnTriggerEnter2D(Collider2D thing) 
    {
        if(thing.gameObject.CompareTag("ConsumableObject")) 
        {
            Destroy(thing.gameObject);
        }
    }
*/
    //Checks if character is grounded
    public LayerMask groundedCheckLayerMask;
    private bool CheckForGrounded()
    {
        Vector2 size = new Vector2(charCollider.size.x -0.1f, charCollider.size.y - 0.2f);
        RaycastHit2D groundRaycast = Physics2D.BoxCast(charCollider.bounds.center, size, 0, Vector2.down, 0.2f,groundedCheckLayerMask);
        if(groundRaycast.collider != null)
        {
            return true;
        }
        return false;
    }

    //Accessors/modifiers
    public bool GetHorizontalMovementActive()
    {
        return horizontalMovementActive;
    }

    public void SetHorizontalMovementActive(bool val)
    {
        horizontalMovementActive = val;
    }

    public bool GetIsGrounded()
    {
        
        return isGrounded;
    }

    public int GetDirection()
    {
        return direction;
    }

    public void SetDirection(int dir)
    {
         direction = dir;
    }

    //get input for movement checking
    public static float getXinput()
    {
        return xInput;
        
    }


    
    
}
