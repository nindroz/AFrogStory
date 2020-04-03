using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingBridge : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rb1;
    public Rigidbody2D rb2;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Break());
        }
    }

    IEnumerator Break()
    {
        yield return new WaitForSeconds(0.35f);
        rb1.bodyType = RigidbodyType2D.Dynamic;
        rb2.bodyType = RigidbodyType2D.Dynamic;
        rb1.gameObject.layer = 8;
        rb2.gameObject.layer = 8;
    }
}
