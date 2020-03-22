using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueScript : MonoBehaviour
{
    //Grabs objects of MoveableObject tag & layer

    //Singleton
    public static TongueScript charTongueScript;
    private void Awake()
    {
        charTongueScript = this;
    }

    public GameObject parent;

    private float maxTongueJointDistance = 0.25f;
    private float currentTongueJointDistance = 0.25f;

    public float maxDist;
    private int jointChangeInterval = 20;//Number of joints per wait period when instantiating/removing, higher number means faster

    public GameObject tongueJointPrefab;
    public List<GameObject> tongueJoints;
    private int jointCount;

    private bool tongueCooldown = false;
    private float tongueCooldownTime = 1f;

    private Vector2 tongueBaseOffset = new Vector2(0,-1f);//Tongue offset from center of character

    private float tongueMaxMoveForce = 450f;

    private float minTongueGrabDistance = 1.5f;//When grabbing, pulls to this distance away from char

    private float maxTongueJointDrag = 4f;//Drag increases w/ further out joints
    private float minTongueJointDrag = 0.2f;

    private Vector2 initialGrabMouseScreenPos;
    private float mousePositionThrowMinThreshhold = 3f;
    private bool mousePositionMinThreshholdReached = false;//When mouse moves past this point, begin checking velocity
    private float mousePositionThrowMaxThreshhold = 8f;    //When mouse moves past this point, throw
    private float mouseGrabTime = 0;

    private float maxThrowVel = 85f;
    private float throwVelModifier = 1.1f;


    private GameObject grabbedObject;

    private bool tongueOut = false;
    private void Start()
    {
        tongueJoints = new List<GameObject>();
        Physics2D.IgnoreLayerCollision(9, 9);
        Physics2D.IgnoreLayerCollision(9, 10);
        Physics2D.IgnoreLayerCollision(9, 11);
    }
    // Update is called once per frame
    void Update()
    {
        //On initial mousedown, start tongue 
        if(Input.GetMouseButtonDown(0) && !tongueCooldown && testCharMovementScript.charMoveScript.GetHorizontalMovementActive())
        {
            testCharMovementScript.charMoveScript.SetHorizontalMovementActive(false);
            //Gets vector from mouse, limits to maxDist if larger
            tongueCooldown = true;
            Vector2 targetVector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - (transform.position+(Vector3)tongueBaseOffset);
            if(targetVector.magnitude > maxDist)
            {
                targetVector = targetVector.normalized * maxDist;
            }

            StartCoroutine(GenerateTongue((Vector2)transform.position + tongueBaseOffset, targetVector));
        }
        //if mouse not being held down, attempt retract 
        if(!Input.GetMouseButton(0) && tongueOut && tongueCooldown)
        {
            tongueOut = false;
            StartCoroutine(RetractTongue());
        }
        if (tongueOut)
        {
            currentTongueJointDistance = Mathf.Min(maxTongueJointDistance, GetMouseDistance()/ (jointCount - 1));
            TonguePhysics(currentTongueJointDistance);
        }
    }
    private void TonguePhysics(float _currentTongueJointDistance)
    {
        int l = tongueJoints.Count;
        if (l <= 0)
            return;
        //If tongue is out, constrain the joints together to create illusion of one long-ass tongue  
        //Also apply a force in the direction of the mouse, higher force when mouse is further away 
        float maxMouseDist = 5f;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 targetDiff = mousePos - (Vector2)transform.position;        
        //Binds first joint to fixed position
        GameObject joint = tongueJoints[0];
        Vector2 pos = joint.transform.position;
        Vector2 diff = pos - ((Vector2)transform.position + tongueBaseOffset);
        joint.transform.position = (Vector2)transform.position + diff.normalized * _currentTongueJointDistance;
        //Mouse force
        Vector2 mouseDiff = (mousePos - (Vector2)joint.transform.position);
        joint.GetComponent<Rigidbody2D>().AddForce(mouseDiff.normalized * tongueMaxMoveForce* (mouseDiff.magnitude /maxMouseDist));
        //Binds other joints to previous joint
        for (int x = 1; x < l; x++)
        {
            joint = tongueJoints[x];
            pos = joint.transform.position;
            diff = pos - (Vector2)tongueJoints[x - 1].transform.position;
            joint.transform.position = (Vector2)tongueJoints[x - 1].transform.position + diff.normalized * _currentTongueJointDistance;
            //Apply force
            mouseDiff = (mousePos - (Vector2)joint.transform.position);

            joint.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            joint.GetComponent<Rigidbody2D>().AddForce(mouseDiff.normalized * tongueMaxMoveForce*(mouseDiff.magnitude/maxMouseDist));
        }
        //Grabbed object inherits velocity from mouse movement  
        if(grabbedObject != null)
        {
            Vector2 grabMouseScreenPos = Input.mousePosition;
            Vector2 grabMouseDiff = (grabMouseScreenPos - initialGrabMouseScreenPos) * 2 * Camera.main.orthographicSize/Screen.height;//Finds mouse movement from origin, converts to units
            //Starts calculating velocity when over min threshhold
            if (grabMouseDiff.magnitude >= mousePositionThrowMinThreshhold && !mousePositionMinThreshholdReached)
            {
                mousePositionMinThreshholdReached = true;
            }
            //Adds to velocity time when min threshhold passed
            else if(mousePositionMinThreshholdReached)
            {
                mouseGrabTime += Time.deltaTime;
            }
            //One out of throw zone, chucc and retracc
            if (grabMouseDiff.magnitude >= mousePositionThrowMaxThreshhold)
            {
                Vector2 vel = grabMouseDiff / mouseGrabTime;
                vel = vel  * throwVelModifier;
                if (vel.magnitude > maxThrowVel)
                    vel = vel.normalized * maxThrowVel;
                grabbedObject.GetComponent<Rigidbody2D>().velocity = vel;
                grabbedObject = null;
                //Retract
                tongueOut = false;
                StartCoroutine(RetractTongue());
            }
            else
            {
                //Grabbed object sticks to last tongue joint
                grabbedObject.transform.position = tongueJoints[l - 1].transform.position;
            }
        }
    }

    private float GetMouseDistance()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 targetDiff = mousePos - (Vector2)transform.position;
        return targetDiff.magnitude;
    }
    IEnumerator RetractTongue()
    {
        testCharMovementScript.charMoveScript.SetHorizontalMovementActive(true);
        int counter = 0;
        //Retract effect by incrimenting jointDistance down
        currentTongueJointDistance = Mathf.Min(maxTongueJointDistance, GetMouseDistance() / (jointCount - 1));
        for (float x = 0;x <= currentTongueJointDistance - minTongueGrabDistance/jointCount; x+= 0.01f)
        {
            TonguePhysics(currentTongueJointDistance - x);
            if (counter == (int)(jointChangeInterval/10f))
            {
                counter = 0;
                yield return new WaitForFixedUpdate();
            }
            counter++;
        }
        //Removes & destroys all existing tongue joints
        foreach(GameObject j in tongueJoints)
        {
            Destroy(j);
        }
        tongueJoints.Clear();
/*        //Renables collisions with target
        if(grabbedObject != null)
            Physics2D.IgnoreCollision(testCharMovementScript.charCollider, grabbedObject.GetComponent<Collider2D>(),false);*/
        grabbedObject = null;
        yield return new WaitForSeconds(tongueCooldownTime);
        tongueCooldown = false;
    }
    public LayerMask tongueHitMask;
    //Instantiates tongue joints along target vector
    IEnumerator GenerateTongue(Vector2 _tongueSource, Vector2 _targetVector)
    {
        initialGrabMouseScreenPos = Input.mousePosition;
        mouseGrabTime = 0.1f;
        mousePositionMinThreshholdReached = false;

        float dist = _targetVector.magnitude;

        Vector2 size = new Vector2(1.5f, 1.5f);
        float lengthBuffer = 0.5f;
        //Boxcast checks if vector hits any moveable gameobjects/terrain
        RaycastHit2D targetHit = Physics2D.BoxCast(_tongueSource, size, 0f, _targetVector, _targetVector.magnitude + lengthBuffer, tongueHitMask);
        if (targetHit.collider != null)
        {
            //Changes targetvector to reach hit object
            _targetVector = targetHit.point - _tongueSource;
        }

        //Generates joints
        jointCount = (int)(maxDist / maxTongueJointDistance)+1;
        int counter = 0;
        for(int x = 0;x < jointCount;x++)
        {
            GameObject tongueJoint = Instantiate(tongueJointPrefab, Vector2.Lerp(_tongueSource, _tongueSource + _targetVector, x / (float)jointCount),Quaternion.identity);
            tongueJoint.transform.SetParent(parent.transform);
            
            Rigidbody2D jointRb = tongueJoint.GetComponent<Rigidbody2D>();
            jointRb.constraints = RigidbodyConstraints2D.FreezeRotation;
            jointRb.drag = Mathf.Lerp(minTongueJointDrag, maxTongueJointDrag, (float)x / (jointCount-1));
            tongueJoints.Add(tongueJoint);
            if(counter == jointChangeInterval)
            {
                counter = 0;
                yield return new WaitForFixedUpdate();
            }
            counter++;
        }
        if(targetHit.collider == null || !targetHit.collider.gameObject.CompareTag("MoveableObject"))
        {
            StartCoroutine(RetractTongue());
        }
        else
        {
            //Removes collisions with target
            //Physics2D.IgnoreCollision(testCharMovementScript.charCollider, targetHit.collider);
            tongueOut = true;
            grabbedObject = targetHit.collider.gameObject;
        }

    }

    public void SetTongueOut(bool val)
    {
        tongueOut = val;
    }

    public bool GetTongueOut()
    {
        return tongueOut;
    }
}
