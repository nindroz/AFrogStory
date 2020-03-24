
using UnityEngine;

public class FrogManager : MonoBehaviour
{
    //Singleton
    public static FrogManager charFrogManager;
    public static Vector2 respawnPosition;//Respawn/checkpoints

    //spirte render and collider
    public SpriteRenderer render;
    public PolygonCollider2D polygonCollider;
    private void Awake()
    {
        charFrogManager = this;
    }


    void Start()
    {
        respawnPosition = gameObject.transform.position;
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Hitting hazard sends player back to respawn point
        if(collision.gameObject.CompareTag("Hazard"))
        {
            gameObject.transform.position = respawnPosition;

            StartCoroutine(UIManager.uiManager.PlayRespawnOverlay());
        }
    }
}
