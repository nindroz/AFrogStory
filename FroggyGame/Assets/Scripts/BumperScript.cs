using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperScript : MonoBehaviour
{
    public bool isRight;
    public bool isTouchingWall;
    // Start is called before the first frame update
    void Start()
    {
        isTouchingWall = false;
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Platform")
        {
            //Debug.Log("Touching Wall");
            isTouchingWall = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Platform")
        {
            //Debug.Log("Leaving Wall");
            isTouchingWall = false;
        }
    }

    void Update()
    {
        if(isTouchingWall == true)
        {
            
        }
    }

    void FixedUpdate()
    {

    }
}
