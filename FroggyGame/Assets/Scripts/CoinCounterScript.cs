using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCounterScript : MonoBehaviour
{
    public static CoinCounterScript counter;
    public Text text;
    int score;
    // Start is called before the first frame update
    void Start()
    {
        if(counter == null)
        {
            counter = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //coin value is value to add each time a coin is picked up!!
    public void changeScore(int coinValue)
    {
        score += coinValue;
        text.text = "COINS: " + score.ToString();
    }
}
