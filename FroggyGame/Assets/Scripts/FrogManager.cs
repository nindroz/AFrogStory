using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogManager : MonoBehaviour
{
    //Singleton
    public static FrogManager charFrogManager;
    public static Vector2 respawnPosition;//Respawn/checkpoints
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
