
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

    //animation grounding
    public bool prevGounded;

    //Timers for powerups
    private float ghostPowerupDuration = 10f;
    private float ghostPowerupTimer = 0;
    private float firedashPowerupDuration = 10f;
    private float firedashPowerupTimer = 0;
    private float glidePowerupDuration = 10f;
    private float glidePowerupTimer = 0;

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
                firedashPowerupTimer = 0;
                PitayaManagerScript.fireDashPowerupScript.SetFiredashPowerup(false);
                particleManager.PlayFiredashPowerupEffectDeactivated();
                particleManager.SetPlayFiredashPowerupEffectPassive(false);
            }
        }
        if (glidePowerupTimer > 0)
        {
            glidePowerupTimer -= Time.deltaTime;
            if (glidePowerupTimer <= 0)
            {
                glidePowerupTimer = 0;
                StartCoroutine(GlidePowerupControlScript.glidePowerupScript.DeactivateGlidePowerup());
            }
        }

        //animator stuff
       
        
        Debug.Log(testCharMovementScript.isJump);
        prevGounded = testCharMovementScript.isGrounded;
        if (testCharMovementScript.isGrounded == true && prevGounded == false)
        {
            testCharMovementScript.isJump = false;
        }
        animator.SetFloat("speed", Mathf.Abs(testCharMovementScript.getXinput()));
        animator.SetBool("isJump", testCharMovementScript.isJump);



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
        //hitting enemy does as well 
        if (collision.gameObject.CompareTag("enemy"))
        {
            gameObject.transform.position = respawnPosition;

            StartCoroutine(UIManager.uiManager.PlayRespawnOverlay());
        }
        //Activate ghost pwerup
        if (collision.gameObject.CompareTag("GhostPowerupFruit"))
        {
            ghostPowerupTimer = ghostPowerupDuration;
            Destroy(collision.gameObject);
            particleManager.PlayGhostPowerupEffectActivated();
            GhostPowerupScript.charGhostPowerupScript.SetGhostPowerup(true);
            particleManager.SetPlayGhostPowerupEffectActive(true);
        }
        //Activate firedash pwerup
        if (collision.gameObject.CompareTag("FiredashPowerupFruit"))
        {
            firedashPowerupTimer = firedashPowerupDuration;
            Destroy(collision.gameObject);
            particleManager.PlayFiredashPowerupEffectActivated();
            PitayaManagerScript.fireDashPowerupScript.SetFiredashPowerup(true);
            particleManager.SetPlayFiredashPowerupEffectPassive(true);
        }
        //Activate glide pwerup
        if (collision.gameObject.CompareTag("GlidePowerupFruit"))
        {
            glidePowerupTimer = glidePowerupDuration;
            Destroy(collision.gameObject);
            StartCoroutine(GlidePowerupControlScript.glidePowerupScript.ActivateGlidePowerup());
        }
    }
}
