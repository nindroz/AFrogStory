using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlidePowerupControlScript : MonoBehaviour
{
    //BUUUURD IS THE WUUUURD
    //Singleton
    public static GlidePowerupControlScript glidePowerupScript;

    //Components & References
    Rigidbody2D glideRb;
    BoxCollider2D glideCollider;
    public GameObject glideNpc;

    private GameObject charGameObject;
    private Rigidbody2D charRb;
    
    private bool isGlidePowerupActive = false;//Is powerup currently active
    private bool isGlidePowerupReady = true;//Is the powerup ready to be reactivated

    //Public vars
    public Vector2 summonOffset;//Where bird appears on summon relative to player
    public Vector2 attachOffset;//Offset from frog center where bird attaches to 

    //Control vars

    private float xInput;

    //Animation frames

    public Sprite flapSprite;
    public Sprite glideSprite;
    public SpriteRenderer spriteRen;
    void Awake()
    {
        glidePowerupScript = this;
        //Gets components
        glideRb = glideNpc.GetComponent<Rigidbody2D>();
        glideCollider = glideNpc.GetComponent<BoxCollider2D>();
        glideRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        //Off by default
        glideNpc.SetActive(false);
        Physics2D.IgnoreLayerCollision(12, 11);
    }


    private void Start()
    {
        //Gets player gameObject
        charGameObject = testCharMovementScript.charMoveScript.gameObject;
        charRb = charGameObject.GetComponent<Rigidbody2D>();
    }
    public IEnumerator ActivateGlidePowerup()//On activation
    {
        if (isGlidePowerupReady)
        {
            isGlidePowerupReady = false;
            //Freezes player rigidbody
            charRb.constraints = RigidbodyConstraints2D.FreezeAll;
            //Dropdown Animation
            Vector2 targetPos = charRb.position + attachOffset;
            Vector2 initPos = targetPos + summonOffset;
            glideNpc.transform.position = initPos; 

            glideNpc.SetActive(true);
            float dist = summonOffset.magnitude;
            //DROPDOWN LOOP YEAHHHHHH
            for(float x = 0;x <= Mathf.PI/2;x+= Mathf.PI/50)
            {
                glideNpc.transform.position = Vector2.Lerp(initPos, targetPos, Mathf.Sin(x));
                yield return new WaitForFixedUpdate();
            }
            //Locks into place
            glideNpc.transform.position = targetPos;
            isGlidePowerupActive = true;

        }
    }

    public IEnumerator DeactivateGlidePowerup()
    {
        if (isGlidePowerupActive)
        {
            isGlidePowerupActive = false;
            //unFreezes player rigidbody
            charRb.constraints = RigidbodyConstraints2D.FreezeRotation;

            //Dropdown Animation
            Vector2 initialPos = charRb.position + attachOffset;
            Vector2 targetPos = initialPos + summonOffset;
            float dist = summonOffset.magnitude;
            //DrOPUP LOOP YEAHHH
            for (float x = 0; x <= Mathf.PI / 2; x += Mathf.PI / 50)
            {
                glideNpc.transform.position = Vector2.Lerp(initialPos, targetPos, (1f-Mathf.Cos(x)));
                yield return new WaitForFixedUpdate();
            }
            //Deactivates gameObjects and states
            glideNpc.SetActive(false);
            isGlidePowerupReady = true;
        }
    }
    private float wingFlapCooldown = 0.75f;
    private float wingFlapTimer = 0;
    public float flapForce = 50f;

    //Anim
    private float flapAnimDuration = 0.5f;
    private float flapAnimTimer = 0f;
    public float horizontalVelocity;
    void Update()
    {


        //Controls
        xInput = Input.GetAxis("Horizontal");
        //Horizontal
        if (xInput == 1)
            spriteRen.flipX = true;
        else if (xInput == -1)
            spriteRen.flipX = false;

        glideRb.velocity = new Vector2(xInput * horizontalVelocity,glideRb.velocity.y);

        //Rising
        wingFlapTimer -= Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Space) && wingFlapTimer <= 0)
        {
            spriteRen.sprite = flapSprite;//For flapping frames
            flapAnimTimer = flapAnimDuration;

            wingFlapTimer = wingFlapCooldown;
            glideRb.AddForce(new Vector2(0,flapForce * glideRb.mass));
        }

        if(flapAnimTimer > 0)
        {
            flapAnimTimer -= Time.deltaTime;
            if(flapAnimTimer <= 0)
            {
                spriteRen.sprite = glideSprite;
            }
        }

    }
    private void FixedUpdate()
    {
        if (isGlidePowerupActive)
        {
            //Locks player to glide npc
            charRb.position = (Vector2)glideNpc.transform.position - attachOffset;
        }
    }
}
