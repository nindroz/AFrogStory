using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    //Singleton
    public static ParticleManager particleManager;
    private void Awake()
    {
        particleManager = this;
    }
    //Particle effects
    public ParticleSystem ghostPowerupEffect;
    public ParticleSystem ghostPowerupEffectDeactivated;
    void Start()
    {
        //Stops all particle systems from playing
        ghostPowerupEffect.Stop();
        ghostPowerupEffectDeactivated.Stop();
    }

    public void PlayGhostPowerupEffect()
    {
        ghostPowerupEffect.Play();
    }
    public void PlayGhostPowerupEffectDeactivated()
    {
        ghostPowerupEffectDeactivated.Play();
    }
}
