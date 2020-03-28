using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class jumpBarScript : MonoBehaviour
{

    public Image jumpBarFill;
    public Image jumpBarBackground;

    public float maxScale;// x o/ fill Scale in which alpha will become 1
    // Update is called once per frame
    void Update()
    {
        float ratio = jumpBarFill.gameObject.transform.localScale.x / maxScale;
        Color c = jumpBarFill.color;
        c.a = Mathf.Lerp(c.a,ratio,5f*Time.deltaTime);
        jumpBarFill.color = c;
        c = jumpBarBackground.color;
        c.a = Mathf.Lerp(c.a, ratio, 5f * Time.deltaTime);
        jumpBarBackground.color = c;
    }
}
