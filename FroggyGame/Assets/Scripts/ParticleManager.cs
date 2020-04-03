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
    public ParticleSystem ghostPowerupEffectActivated;
    public ParticleSystem ghostPowerupEffectDeactivated;
    public ParticleSystem ghostPowerupEffectActive;

    public ParticleSystem FiredashPowerupEffectActivated;
    public ParticleSystem FiredashPowerupEffectDeactivated;
    public ParticleSystem FiredashPowerupEffectActive;
    public ParticleSystem FiredashPowerupEffectActivePassive;

    public ParticleSystem DurianPowerupEffectActive;

    public void PlayGhostPowerupEffectActivated()
    {
        ghostPowerupEffectActivated.Play();
    }
    public void PlayGhostPowerupEffectDeactivated()
    {
        ghostPowerupEffectDeactivated.Play();
    }
    public void SetPlayGhostPowerupEffectActive(bool var)
    {
        if (var)
            ghostPowerupEffectActive.Play();
        else
            ghostPowerupEffectActive.Stop();
    }

    public void PlayFiredashPowerupEffectActivated()
    {
        FiredashPowerupEffectActivated.Play();
    }
    public void PlayFiredashPowerupEffectDeactivated()
    {
        FiredashPowerupEffectDeactivated.Play();
    }
    public void PlayFiredashPowerupEffectActive()
    {
        FiredashPowerupEffectActive.Play();
    }
    public void SetPlayFiredashPowerupEffectPassive(bool var)
    {
        if (var)
            FiredashPowerupEffectActivePassive.Play();
        else
            FiredashPowerupEffectActivePassive.Stop();
    }

    public void SetPlayDurianPowerupEffectActive(bool var)
    {
        if (var)
            DurianPowerupEffectActive.Play();
        else
            DurianPowerupEffectActive.Stop();
    }

}
