using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRambutan : MonoBehaviour
{
    public GameObject wjp;
    public bool isDeployed;
    public GameObject wallJumpPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        isDeployed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!isDeployed)
            {
                Debug.Log("Deploying The WallJump Prefab");
                //GameObject wallJumpPrefab;
                wallJumpPrefab = Instantiate(wjp, transform.position, Quaternion.identity) as GameObject;
                wallJumpPrefab.transform.parent = transform;
                
                isDeployed = true;
            } 
            else
            {

                Debug.Log("Retracting The WallJump Prefab");
                foreach (var name in GameObject.FindObjectsOfType<RambutanManagerScript>())
                {
                    GameObject go = GetComponentInChildren<RambutanManagerScript>().gameObject;
                    //if the tree exist then destroy it
                    if (go)
                        Destroy(go.gameObject);
                }
                isDeployed = false;
                
            }
            
        }
    }
}
