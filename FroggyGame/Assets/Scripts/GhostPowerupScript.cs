using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPowerupScript : MonoBehaviour
{
    //Singleton
    public static GhostPowerupScript charGhostPowerupScript;
    public LayerMask activeMask;
    public LayerMask inactiveMask;

    //bool to see if ghosting
    public static bool isGhosting = false;
    void Awake()
    {
        charGhostPowerupScript = this;
    }

    public void SetGhostPowerup(bool var)
    {
        isGhosting = var;
        Physics2D.IgnoreLayerCollision(10, 12, var);
        Physics2D.IgnoreLayerCollision(9, 12, var);
        if (var ==true)
        {
            //Change masks to ignore layer
            TongueScript.charTongueScript.tongueHitMask = activeMask;
            testCharMovementScript.charMoveScript.groundedCheckLayerMask = activeMask;
        }
        else
        {
            TongueScript.charTongueScript.tongueHitMask = inactiveMask;
            testCharMovementScript.charMoveScript.groundedCheckLayerMask = inactiveMask;
        }
    }

    //Testing

}
