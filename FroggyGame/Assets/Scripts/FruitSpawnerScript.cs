using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawnerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject fruitPrefab;

    private GameObject currentFruit;
    SpriteRenderer spriteRen;
    Rigidbody2D rb;

    //States
    bool isTaken = false;
    bool isCountingDown = false;

    public float respawnTime = 15f;
    private float respawnTimer = 0;

    private float storedGravity;

    void Start()
    {
        StartCoroutine(RespawnFruit());
    }

    // Update is called once per frame
    void Update()
    {
        if(!isTaken && currentFruit != null && gameObject.transform.position != currentFruit.transform.position)
        {
            isTaken = true;
            rb.gravityScale = storedGravity;
        }

        if(currentFruit == null && !isCountingDown)
        {
            isCountingDown = true;
            //Respawn timer
            respawnTimer = respawnTime;
        }
        if(isCountingDown)
        {
            respawnTimer -= Time.deltaTime;
            if(respawnTimer <= 0)
                StartCoroutine(RespawnFruit());
        }
    }

    public IEnumerator RespawnFruit()
    {
        isCountingDown = false;
        isTaken = false;
        currentFruit = Instantiate(fruitPrefab, gameObject.transform.position, Quaternion.identity);
        currentFruit.GetComponent<Collider2D>().enabled = false;//Prevents being taken in transition, causing null object
        spriteRen = currentFruit.GetComponent<SpriteRenderer>();
        rb = currentFruit.GetComponent<Rigidbody2D>();
        storedGravity = rb.gravityScale;
        rb.gravityScale = 0;

        Color c = spriteRen.color;
        for(int x = 0;x <= 10;x++)
        {
            c.a = x / 10f;
            spriteRen.color = c;
            yield return new WaitForSeconds(0.05f);
        }
        currentFruit.GetComponent<Collider2D>().enabled = true;
    }
}
