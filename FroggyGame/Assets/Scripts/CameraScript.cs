using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject cameraSubject;
    private Camera _camera;
    public int cameraMode = 0;//0 is follow player
    private Rigidbody2D cameraRb;
    //bounds
    private bool followBounds = false;
    private float lowerBound, upperBound, leftBound, rightBound;
    void Start()
    {
        _camera = gameObject.GetComponent<Camera>();
        cameraRb = gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //Follows player
        if(cameraMode == 0)
        {

            Vector2 targetpos = cameraSubject.transform.position;
            float dist = (targetpos - (Vector2)transform.position).magnitude;
            gameObject.transform.position = Vector2.Lerp(transform.position, targetpos, (0.8f + dist)*2 * Time.deltaTime);

            float yForce = cameraSubject.GetComponent<Rigidbody2D>().velocity.y;
            if (cameraSubject.GetComponent<Rigidbody2D>().velocity.y > 0)
            {
                yForce /= 4f;
            }
            cameraRb.AddForce(new Vector2(cameraSubject.GetComponent<Rigidbody2D>().velocity.x * 0.5f, yForce));
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

            float xPos = transform.position.x;
            float yPos = transform.position.y;
            //clamping
            if (transform.position.x + screenWidthBound > rightBound)
                xPos = rightBound - screenWidthBound;
            else if (transform.position.x - screenWidthBound < leftBound)
                xPos = leftBound + screenWidthBound;
            if (transform.position.y + screenHeightBound > upperBound)
                yPos = upperBound - screenHeightBound;
            else if (transform.position.y - screenHeightBound < lowerBound)
                yPos = lowerBound + screenHeightBound;

            transform.position = new Vector2(xPos, yPos);
        }

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10f);

    }

    public void SetBounds(float left, float right, float upper, float lower)
    {
        lowerBound = lower;
        upperBound = upper;
        leftBound = left;
        rightBound = right;
    }

    public void SetFollowBounds(bool val)
    {
        followBounds = val;
    }
}
