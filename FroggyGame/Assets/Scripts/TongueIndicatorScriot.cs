using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueIndicatorScriot : MonoBehaviour
{
    //Singleton
    public static TongueIndicatorScriot tongueIndicatorScriot;

    public LineRenderer lineRenInner;
    public LineRenderer lineRenOuter;
    Material matIn;
    Material matOut;
    public Color color;
    int segments = 360;//1 degree per segment
    bool showing = false;
    bool inTransition = false;
    // Start is called before the first frame update
    void Awake()
    {
        tongueIndicatorScriot = this;
        matIn = lineRenInner.material;
        matOut = lineRenInner.material;
        lineRenInner.positionCount = (segments);
        lineRenOuter.positionCount = (segments);
        matIn.color = color;
        matOut.color = color;
    }

    public void DrawArcInner(float range,Vector2 center)
    {
        float x, y;
        float cAngle = 0;
        for (int c = -segments / 2; c < segments / 2; c++)
        {
            cAngle = Mathf.Deg2Rad * c;
            x = Mathf.Cos(cAngle) * range;
            y = Mathf.Sin(cAngle) * range;

            lineRenInner.SetPosition(c + segments / 2, new Vector2(x, y) + center);
        }
    }

    public void DrawArcOuter(float range, Vector2 center)
    {
        float x, y;
        float cAngle = 0;
        for (int c = -segments / 2; c < segments / 2; c++)
        {
            cAngle = Mathf.Deg2Rad * c;
            x = Mathf.Cos(cAngle) * range;
            y = Mathf.Sin(cAngle) * range;

            lineRenOuter.SetPosition(c + segments / 2, new Vector2(x, y) + center);
        }
    }
    public IEnumerator Hide()
    {
        lineRenInner.enabled = false;
        lineRenOuter.enabled = false;
        while (inTransition)
        {
            yield return new WaitForFixedUpdate();
        }
        if (showing)
        {
            inTransition = true;
            Color col = matIn.color;
            for (int x = 0; x <= 10; x++)
            {
                col.a = 1 - x / 10f;
                matIn.color = (col);
                matOut.color = (col);
                yield return new WaitForFixedUpdate();
            }
            inTransition = false;
            showing = false;
        }
    }
    public IEnumerator Show()
    {
        lineRenInner.enabled = true;
        lineRenOuter.enabled = true;
        if (!showing && !inTransition)
        {
            inTransition = true;
            Color col = matIn.color;
            for (int x = 10; x >= 0; x--)
            {
                col.a = 1 - x / 10f;
                matOut.color = (col);
                yield return new WaitForFixedUpdate();
            }
            inTransition = false;
            showing = true;
        }
    }
    public bool getShowing()
    {
        return showing;
    }
}
