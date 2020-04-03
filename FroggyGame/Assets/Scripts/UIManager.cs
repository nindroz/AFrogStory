using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    public void Start()
    {
        StartCoroutine(UIManager.uiManager.PlayRespawnOverlay());
    }
    public IEnumerator PlayRespawnOverlay()
    {
        Color c = respawnOverlay.color;
        for(int x = 0;x <= 15;x++)
        {
            c.a = (1 - x / 15f);
            respawnOverlay.color = c;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public IEnumerator PlaySceneChangeOverlay(string sceneName)
    {
        Color c = respawnOverlay.color;
        for (int x = 0; x <= 50; x++)
        {
            c.a = (x / 50f);
            respawnOverlay.color = c;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);
    }
}
