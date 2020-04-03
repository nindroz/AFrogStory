using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : MonoBehaviour
{
    // Start is called before the first frame update
    void FixedUpdate()
    {
        gameObject.GetComponent<Rigidbody2D>().AddTorque(-30f);
    }

      
}
