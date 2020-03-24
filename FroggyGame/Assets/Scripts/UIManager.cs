using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Singleton
    public static UIManager uiManager;

    private void Awake()
    {
        uiManager = this;
    }

    //UI components
    public Image respawnOverlay;

    public IEnumerator PlayRespawnOverlay()
    {
        Color c = respawnOverlay.color;
        for(int x = 0;x <= 10;x++)
        {
            c.a = (1 - x / 10f);
            respawnOverlay.color = c;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
