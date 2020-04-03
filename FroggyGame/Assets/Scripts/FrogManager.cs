
using UnityEngine;

public class FrogManager : MonoBehaviour
{
    //Singleton
    public static FrogManager charFrogManager;
    public static Vector2 respawnPosition;//Respawn/checkpoints

    //spirte render and collider and animator
    public SpriteRenderer render;
    public PolygonCollider2D polygonCollider;
    public Animator animator;

    // frog sprites
    public Sprite transparentFrog;
    public Sprite normalFrog;

       
    //script reference
    public testCharMovementScript move;

    //prevents multiple powerups
    private bool isPowerupActive = false;

    //Timers for powerups
    private float ghostPowerupDuration = 10f;
    private float ghostPowerupTimer = 0;
    private float firedashPowerupDuration = 10f;
    private float firedashPowerupTimer = 0;
    private float glidePowerupDuration = 10f;
    private float glidePowerupTimer = 0;
    private float durianPowerupDuration = 10f;
    private float durianPowerupTimer = 0;
    private bool durianPowerupActive = false;
    private float durianPowerupMovespeedSave;//Saves original movespeed
    private float durianPowerupMovespeed = 12;

    ParticleManager particleManager;



    void Awake()
    {
        charFrogManager = this;
        respawnPosition = gameObject.transform.position;
        particleManager = ParticleManager.particleManager;
    }

    // Update is called once per frame
    void Update()
    {
        int dir = testCharMovementScript.charMoveScript.GetDirection();
        //filps sprite render and collider
        if (dir == 1)
        {
            render.flipX = false;
            //polygonCollider.transform.localScale = new Vector2(-1, 1);

        }
        if (dir == -1)
        {
            render.flipX = true;
            //polygonCollider.transform.localScale = new Vector2(1, 1);

        }

        //changes opacxity when ghosting
        if (GhostPowerupScript.isGhosting)
        {
            render.sprite = transparentFrog;
        }
        if (!GhostPowerupScript.isGhosting)
        {
            render.sprite = normalFrog;
        }

        //Timers for powerups
        if (ghostPowerupTimer > 0)
        {
            ghostPowerupTimer -= Time.deltaTime;
            if (ghostPowerupTimer <= 0)
            {
                isPowerupActive = false;
                ghostPowerupTimer = 0;
                GhostPowerupScript.charGhostPowerupScript.SetGhostPowerup(false);
                particleManager.PlayGhostPowerupEffectDeactivated();
                particleManager.SetPlayGhostPowerupEffectActive(false);
            }
        }
        if (firedashPowerupTimer > 0)
        {
            firedashPowerupTimer -= Time.deltaTime;
            if (firedashPowerupTimer <= 0)
            {
                isPowerupActive = false;
                firedashPowerupTimer = 0;
                PitayaManagerScript.fireDashPowerupScript.SetFiredashPowerup(false);
                particleManager.PlayFiredashPowerupEffectDeactivated();
                particleManager.SetPlayFiredashPowerupEffectPassive(false);
                animator.SetBool("isRed",false);

            }
        }
        if (glidePowerupTimer > 0)
        {
            glidePowerupTimer -= Time.deltaTime;
            if (glidePowerupTimer <= 0)
            {
                isPowerupActive = false;
                glidePowerupTimer = 0;
                StartCoroutine(GlidePowerupControlScript.glidePowerupScript.DeactivateGlidePowerup());
            }
        }
        if (durianPowerupTimer > 0)
        {
            durianPowerupTimer -= Time.deltaTime;
            if (durianPowerupTimer <= 0)
            {
                isPowerupActive = false;
                durianPowerupTimer = 0;
                durianPowerupActive = false;
                testCharMovementScript.charMoveScript.moveVelocity = durianPowerupMovespeedSave;//Sets movespeed back to normal
                particleManager.SetPlayDurianPowerupEffectActive(false);
            }
        }

        //animator stuff


        //checks if it hits the ground after a jump
        if (testCharMovementScript.isGrounded == true && testCharMovementScript.prevGrounded == false)
       {
            testCharMovementScript.isJump = false;
       }
        testCharMovementScript.prevGrounded = testCharMovementScript.isGrounded;

        //if (testCharMovementScript.isGrounded == false&& PitayaManagerScript.isDashing == false)
        //{
           //testCharMovementScript.isJump = true;
        //}
        Debug.Log(testCharMovementScript.isJump);
        animator.SetBool("isJump", testCharMovementScript.isJump);

        //for move animation
        if ( testCharMovementScript.isJump==false)
        {
            Debug.Log(testCharMovementScript.getXinput());
            animator.SetFloat("speed", Mathf.Abs(testCharMovementScript.getXinput()));

            
        }
        
        



    }
    //Manages collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Hitting hazard sends player back to respawn point
        if (collision.gameObject.CompareTag("Hazard"))
        {
            gameObject.transform.position = respawnPosition;

            StartCoroutine(UIManager.uiManager.PlayRespawnOverlay());
        }
        //hitting enemy does as well , unless durian is active
        if (collision.gameObject.CompareTag("enemy") && !durianPowerupActive)
        {
            gameObject.transform.position = respawnPosition;

            StartCoroutine(UIManager.uiManager.PlayRespawnOverlay());
        }
        //Activate ghost pwerup
        if (collision.gameObject.CompareTag("GhostPowerupFruit") && (!isPowerupActive || ghostPowerupTimer > 0))
        {
            isPowerupActive = true;
            ghostPowerupTimer = ghostPowerupDuration;
            Destroy(collision.gameObject);
            particleManager.PlayGhostPowerupEffectActivated();
            GhostPowerupScript.charGhostPowerupScript.SetGhostPowerup(true);
            particleManager.SetPlayGhostPowerupEffectActive(true);
        }
        //Activate firedash pwerup
        if (collision.gameObject.CompareTag("FiredashPowerupFruit") && (!isPowerupActive || firedashPowerupTimer > 0))
        {
            isPowerupActive = true;
            firedashPowerupTimer = firedashPowerupDuration;
            Destroy(collision.gameObject);
            particleManager.PlayFiredashPowerupEffectActivated();
            PitayaManagerScript.fireDashPowerupScript.SetFiredashPowerup(true);
            particleManager.SetPlayFiredashPowerupEffectPassive(true);
            animator.SetBool("isRed", true);
        }
        //Activate glide pwerup
        if (collision.gameObject.CompareTag("GlidePowerupFruit") && (!isPowerupActive || glidePowerupTimer > 0))
        {
            isPowerupActive = true;
            glidePowerupTimer = glidePowerupDuration;
            Destroy(collision.gameObject);
            StartCoroutine(GlidePowerupControlScript.glidePowerupScript.ActivateGlidePowerup());
        }
        //Activate durian pwerup
        if (collision.gameObject.CompareTag("DurianPowerupFruit") && (!isPowerupActive || durianPowerupTimer > 0))
        {
            durianPowerupActive = true;//for immunity against enemies
            isPowerupActive = true;
            durianPowerupTimer = durianPowerupDuration;
            Destroy(collision.gameObject);
            //Slows down during durian powerup
            durianPowerupMovespeedSave = testCharMovementScript.charMoveScript.moveVelocity;
            testCharMovementScript.charMoveScript.moveVelocity = durianPowerupMovespeed;

            particleManager.SetPlayDurianPowerupEffectActive(true);
        }
        
    }
}
