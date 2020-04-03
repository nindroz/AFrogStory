using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioManager;

    // Start is called before the first frame update

    //Audio components
    public AudioSource rainSound;

    public AudioSource[] jumpSounds = new AudioSource[4];

    public AudioSource deathSound;
    public AudioSource twigSound;
    public AudioSource fireDashSound;


    void Awake()
    {
        audioManager = this;
    }

    public void PlayJumpSound()
    {
        int n = Random.Range(0, 4);
        jumpSounds[n].Play();
    }


    public void PlayDeathSound()
    {
        deathSound.Play();
    }

    public void PlayTwigSound()
    {
        twigSound.Play();
    }

    public IEnumerator PlayFiredashSound(float time)
    {
        fireDashSound.Play();
        yield return new WaitForSeconds(time);
        fireDashSound.Stop();
    }

    // Update is called once per frame

}
