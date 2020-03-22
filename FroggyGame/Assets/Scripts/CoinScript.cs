using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public int coinValue = 1;

        // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D thing)
    {
        if(thing.gameObject.CompareTag("Player"))
        {
            CoinCounterScript.counter.changeScore(coinValue);
        }
    }
}
