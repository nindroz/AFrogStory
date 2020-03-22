using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turningScript : MonoBehaviour
{
<<<<<<< HEAD

    public GameObject firePoint;
    public GameObject player;

=======
   
    GameObject player;
>>>>>>> long-ranged-dart

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
        if (testCharMovementScript.direction == 1)
        {
            firePoint.transform.position = new Vector2(firePoint.transform.position.x + 2, firePoint.transform.position.y);
        }
        else
        {
            firePoint.transform.position = new Vector2(firePoint.transform.position.x-2, firePoint.transform.position.y);
        }
=======
        if (player)
        {

        } 
>>>>>>> long-ranged-dart
    }
}
