using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turningScript : MonoBehaviour
{

    public GameObject firePoint;
    public GameObject player;


    // Update is called once per frame
    void Update()
    {
        if (testCharMovementScript.direction == 1)
        {
            firePoint.transform.position = new Vector2(firePoint.transform.position.x + 2, firePoint.transform.position.y);
        }
        else
        {
            firePoint.transform.position = new Vector2(firePoint.transform.position.x-2, firePoint.transform.position.y);
        }
    }
}
