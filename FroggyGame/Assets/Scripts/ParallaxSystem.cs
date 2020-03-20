using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxSystem : MonoBehaviour
{
    //Add script to camera object for parallax effect on background/foreground

    [System.Serializable]
    public class ParallaxObject
    {
        public float moveRatio;//Speed in which this object moves relative to normal environment; 1 means same speed, 0.4 means 40 percent speed, etc.
        public GameObject spriteObject;
        public int sortingOrder;
    }   
    public ParallaxObject[] parallaxObjects;


    int parallaxObjectCount;
    Vector2 prevCameraPos;
    void Start()
    {
        parallaxObjectCount = parallaxObjects.Length;
        prevCameraPos = transform.position;

        //Sets sorting order
        foreach(ParallaxObject _parObj in parallaxObjects)
        {
            SpriteRenderer sRen = _parObj.spriteObject.GetComponent<SpriteRenderer>();
            if(sRen != null)
            {
                sRen.sortingOrder = _parObj.sortingOrder;
            }
        }
    }

    //Updates positions of background/foreground
    void Update()
    {
        //Finds vector of camera movement
        Vector2 currCameraPos = transform.position;
        Vector2 diff = currCameraPos - prevCameraPos;
        //Moves parallaxObjects in that direction w/ magnitude based on moveRatio
        foreach (ParallaxObject _parObj in parallaxObjects)
        {
            if(_parObj.spriteObject != null)
            {
                _parObj.spriteObject.transform.Translate(diff * (1 - _parObj.moveRatio));
            }
        }
        prevCameraPos = currCameraPos;
    }
}
