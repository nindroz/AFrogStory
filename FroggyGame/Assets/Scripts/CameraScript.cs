using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    //Singleton
    public static CameraScript cameraScript;


    public GameObject cameraSubject;
    private Camera _camera;
    public int cameraMode = 0;//0 is follow player
    private Rigidbody2D cameraRb;
    //bounds
    private bool followBounds = false;
    private float lowerBound, upperBound, leftBound, rightBound;
    //Bounds transitions
    private float boundTransitionTime = 1f;
    private float boundTransitionTimer = 0;
    void Awake()
    {
        cameraScript = this;
        _camera = gameObject.GetComponent<Camera>();
        cameraRb = gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 newPosition = transform.position;
        //Follows player
        if(cameraMode == 0)
        {

            Vector2 targetpos = cameraSubject.transform.position;
            float dist = (targetpos - (Vector2)transform.position).magnitude;
            newPosition = Vector2.Lerp(transform.position, targetpos, (0.3f + dist/3) * Time.deltaTime);

/*            float yForce = cameraSubject.GetComponent<Rigidbody2D>().velocity.y;
            if (cameraSubject.GetComponent<Rigidbody2D>().velocity.y > 0)
            {
                yForce /= 4f;
            }
            cameraRb.AddForce(new Vector2(cameraSubject.GetComponent<Rigidbody2D>().velocity.x * 0.5f, yForce));*/
        }
        else if(cameraMode == 1)//No smooth camera, just normal cam
        {
            gameObject.transform.position = cameraSubject.transform.position;
        }
        //if bounds are active, ensures camera doesn't leave bounds
        if(followBounds)
        {
            float aspRatio = Screen.width / Screen.height;
            float screenWidthBound = _camera.orthographicSize * aspRatio+3;
            float screenHeightBound = _camera.orthographicSize;

            float xPos = newPosition.x;
            float yPos = newPosition.y;
            //clamping
            if (newPosition.x + screenWidthBound > rightBound)
                xPos = rightBound - screenWidthBound;
            else if (newPosition.x - screenWidthBound < leftBound)
                xPos = leftBound + screenWidthBound;
            if (newPosition.y + screenHeightBound > upperBound)
                yPos = upperBound - screenHeightBound;
            else if (newPosition.y - screenHeightBound < lowerBound)
                yPos = lowerBound + screenHeightBound;
            newPosition = new Vector2(xPos, yPos);
            if(boundTransitionTime > 0)
            {
                boundTransitionTimer -= Time.deltaTime;
                newPosition = Vector2.Lerp(transform.position, newPosition, 7f * Time.deltaTime);
            }
        }
        transform.position = newPosition;
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10f);

    }

    public void SetBounds(float left, float right, float upper, float lower)
    {
        boundTransitionTimer = boundTransitionTime;
        lowerBound = lower;
        upperBound = upper;
        leftBound = left;
        rightBound = right;
    }

    public void SetLeftBound(float left)
    {
        boundTransitionTimer = boundTransitionTime;
        leftBound = left;
    }
    public void SetRightBound(float right)
    {
        boundTransitionTimer = boundTransitionTime;
        rightBound = right;
    }
    public void SetUpperBound(float upper)
    {
        boundTransitionTimer = boundTransitionTime;
        upperBound = upper;
    }
    public void SetLowerBound(float lower)
    {
        boundTransitionTimer = boundTransitionTime;
        lowerBound = lower;
    }
    public void SetFollowBounds(bool val)
    {
        followBounds = val;
    }
}
