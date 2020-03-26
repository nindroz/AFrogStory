
using UnityEngine;

public class FrogManager : MonoBehaviour
{
    //Singleton
    public static FrogManager charFrogManager;
    public static Vector2 respawnPosition;//Respawn/checkpoints

    //spirte render and collider
    public SpriteRenderer render;
    public PolygonCollider2D polygonCollider;

    // frog sprites
    public Sprite transparentFrog;
    public Sprite normalFrog;

    //Timers for powerups
    private float ghostPowerupDuration = 10f;
    private float ghostPowerupTimer = 0;
    private float firedashPowerupDuration = 10f;
    private float firedashPowerupTimer = 0;

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
        //filps sprite render and collider
        if (testCharMovementScript.direction == 1)
        {
            render.flipX = false;
            //polygonCollider.transform.localScale = new Vector2(-1, 1);

        }
        if(testCharMovementScript.direction == -1)
        {
            render.flipX = true;
            //polygonCollider.transform.localScale = new Vector2(1, 1);

        }

        //changes opacxity when ghosting
        if (GhostPowerupScript.isGhosting)
        {
            render.sprite = transparentFrog;
        }
        if(!GhostPowerupScript.isGhosting)
        {
            render.sprite = normalFrog;
        }

        //Timers for powerups
        if(ghostPowerupTimer > 0)
        {
            ghostPowerupTimer -= Time.deltaTime;
            if(ghostPowerupTimer <= 0)
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
            }
        }
    }
    //Manages collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Hitting hazard sends player back to respawn point
        if(collision.gameObject.CompareTag("Hazard"))
        {
            gameObject.transform.position = respawnPosition;

            StartCoroutine(UIManager.uiManager.PlayRespawnOverlay());
        }
        //Activate ghost pwerup
        if(collision.gameObject.CompareTag("GhostPowerupFruit"))
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
        }
    }
}
