using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class jumpBarScript : MonoBehaviour
{

    public Image jumpBarFill;
    public Image jumpBarBackground;

    public Gradient g;

    public float maxScale;// x o/ fill Scale in which alpha will become 1
    // Update is called once per frame
    void Update()
    {
        //Color changes as charges up
        float colorRatio = jumpBarFill.gameObject.transform.localScale.x / 1f;
        Color c = g.Evaluate(colorRatio);
        jumpBarFill.color = c;

        //Changes alpha as charges up
        float ratio = jumpBarFill.gameObject.transform.localScale.x / maxScale;
        c = jumpBarFill.color;
        c.a = Mathf.Lerp(c.a,ratio,10f*Time.deltaTime);
        jumpBarFill.color = c;
        c = jumpBarBackground.color;
        c.a = Mathf.Lerp(c.a, ratio, 10f * Time.deltaTime);
        jumpBarBackground.color = c;
    }
}
