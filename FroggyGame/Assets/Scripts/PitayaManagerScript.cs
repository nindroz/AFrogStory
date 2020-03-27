
using UnityEngine;

//Press "E" To Activate It!

public class PitayaManagerScript : MonoBehaviour
{
    //Singleton
    public static PitayaManagerScript fireDashPowerupScript;

    public float dashTime;                  //How long the dash lasts for
    public float dashDelay;                 //Time period in which you cannot dash again.
    public int dashVelocity;                //Speed at which the frog dashes

    private testCharMovementScript charMove;//The character movement script (which is only needed to detect if the player has grounded or not)
    private Rigidbody2D rBody;              //The RigidBody of the player
    private bool isDashing;                 //Shows if the player is currently dashing
    private bool hasGroundedSinceDash;      //Tracks if the player has touched the ground since their  last dash
    private bool canDash;                   //The player can only dash again if they've waited long enough and have touched the ground since their last dash
    private float timeElapsedSinceDash;     //How long it has been since the last dash ended.
    private float currentDashTime;          //How long the player has been dashing for
    private float storedGravity;            //The gravity before it is turned off during the dash
    private int direction;                  //Whether the cursor is pointed left or right of the player before the dash
    private int dir;
    private bool isFiredashActivated = false;//Whether or not powerup is currently active 

       
    void Awake() //Set all the variables
    {
        fireDashPowerupScript = this;
        //Feel free to mess around with the dashTime, dashDelay & dashVelocity!
        dashTime = 0.4f;
        dashDelay = 0.1f;
        dashVelocity = 70;

        charMove = testCharMovementScript.charMoveScript;
        rBody = GetComponent<Rigidbody2D>();
        isDashing = false;
        canDash = true;
        storedGravity = rBody.gravityScale;
        timeElapsedSinceDash = 0f;
        currentDashTime = 0f;
        direction = 1;
    }

    void FindDirection()    //Detects what direction the pointer is relative to the player to choose which direction to dash in
    {
        if (dir == 0)
        {
            direction = 1;
        }
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            direction = -1;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            direction = 1;
        }
        if (Input.GetKeyDown(KeyCode.M) && !isDashing && canDash) //If the player presses M and can go into a dash, put them in that state.
        {
            isDashing = true;                       //Put them in the dash state
            canDash = false;                        //Prohibit them from chaining dashes
            hasGroundedSinceDash = false;           //Reset their grounded sensor
            currentDashTime = 0f;                   //Reset the dash timer
            rBody.velocity = new Vector2(0, 0);     //Stop all velocity acting on the player
            storedGravity = rBody.gravityScale;     //Store the current gravity acting on the player
            rBody.gravityScale = 0;                 //Change the gravity to zero.
            FindDirection();                        //Find out the direction the player is dashing
            dir = direction;                        //
        }  

            ParticleManager.particleManager.PlayFiredashPowerupEffectActive();//Plays particle effect

            dir = direction;                        //

        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            direction = -1;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            direction = 1;
        }
        if (isDashing)
        {
            rBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation; //Freeze their Y Position and Z rotation so they can only move horizontally
            rBody.velocity = new Vector2(dir * dashVelocity, 0);                                          //Set their horizontal velocity
            currentDashTime += Time.deltaTime;                                                                  //Keep track of how long this dash is going on.

            if (currentDashTime >= dashTime)                                //When the dash has gone on for as long as it's supposed to:
            {
                isDashing = false;                                          //Take the player out of the dashed state
                timeElapsedSinceDash = 0;                                   //Start the timer since thier last dash
                rBody.velocity = new Vector2(0, 0);                         //Set their velocity to zero for a cartoonish drop
                rBody.gravityScale = storedGravity;                         //Reset gravity
                rBody.constraints = RigidbodyConstraints2D.FreezeRotation;  //Unfreeze the Y axis from motion.
            }
        }

        if (charMove.GetIsGrounded() && !canDash)            //If the player has touched the ground after dashing...
        {
            hasGroundedSinceDash = true;            //Then that shouldn't stop them from dashing again.
        }

        if (!canDash)                                   //While the player cannot dash...
        {

            if (timeElapsedSinceDash < dashDelay)       //If it is because their cooldown hasn't run out yet...
            {
                timeElapsedSinceDash += Time.deltaTime; //Add time to the cooldown timer
            }

            else                                        //But if the cooldown has run out...
            {
                if (hasGroundedSinceDash)                //And the player has touched the ground since it's last dash...
                {
                    canDash = true;                     //Allow the player to dash again.
                }
            }
        }
    }

    public void SetFiredashPowerup(bool var)
    {
        isFiredashActivated = var;
    }

    public bool GetIsDashing()
    {
        return isDashing;
    }
}