
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {   
        //moves bullet
        rb.velocity = transform.right * Speed * testCharMovementScript.direction;
    }
    //chekcs for bullet hits
    void OnTriggerEnter2D( Collider2D hitInfo)
    {
        Debug.Log(hitInfo.name);
        Destroy(gameObject);
    }

    // Update is called once per frame
   
}
