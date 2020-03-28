using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamespaceTextScript : MonoBehaviour
{
    // Start is called before the first frame update
    //TExt component
    Text textComponent;
    public float timeToAppear;
    private void Awake()
    {
        textComponent = gameObject.GetComponent<Text>();
        textComponent.enabled = false;
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DisplayText());
        }
    }

    IEnumerator DisplayText()
    {
        Color c = textComponent.color;
        c.a = 0;
        textComponent.color = c;
        float interval = timeToAppear / 10f;
        textComponent.enabled = true;
        for (int x = 0;x <= 10;x++)
        {
            c.a = x / 10f;
            textComponent.color = c;
            yield return new WaitForSeconds(interval);
        }
    }
}
