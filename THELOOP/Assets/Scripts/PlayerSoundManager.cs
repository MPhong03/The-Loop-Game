using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    [Header("Normal State Sound")]
    public AudioSource walk;
    public AudioSource sword;
    public AudioSource spear;
    public AudioSource axe;
    public AudioSource jump;
    public AudioSource land;
    public AudioSource dash;
    public AudioSource ouch;
    public AudioSource takedown;

    [Header("Skill Sound")]
    public AudioSource fire;
    public AudioSource heal;
    public AudioSource freeze;
    public AudioSource wind;
    public AudioSource lightning;
    public AudioSource shield;
    public void PlayWalkSound()
    {
        walk.Play();
    }
    public void PlaySwordSound()
    {
        sword.Play();
    }
    public void PlaySpearSound()
    {
        spear.Play();
    }
    public void PlayAxeSound()
    {
        axe.Play();
    }
    public void PlayJumpSound()
    {
        jump.Play();
    }
    public void PlayLandSound()
    {
        land.Play();
    }
    public void PlayDashSound()
    {
        dash.Play();
    }
    public void PlayFireSound()
    {
        fire.Play();
    }
    public void PlayHealSound()
    {
        heal.Play();
    }
    public void PlayFreezeSound()
    {
        freeze.Play();
    }
    public void PlayWindSound()
    {
        wind.Play();
    }
    public void PlayLightningSound()
    {
        lightning.Play();
    }
    public void PlayShieldSound()
    {
        shield.Play();
    }
    public void PlayOuchSound()
    {
        ouch.Play();
    }
    public void PlayTakeDown()
    {
        takedown.Play();
    }

    public void StopWalkSound()
    {
        walk.Stop();
    }
    public void StopSwordSound()
    {
        sword.Stop();
    }
    public void StopSpearSound()
    {
        spear.Stop();
    }
    public void StopAxeSound()
    {
        axe.Stop();
    }
    public void StopJumpSound()
    {
        jump.Stop();
    }
    public void StopLandSound()
    {
        land.Stop();
    }
    public void StopDashSound()
    {
        dash.Stop();
    }
    public void StopFireSound()
    {
        fire.Stop();
    }
    public void StopHealSound()
    {
        heal.Stop();
    }
    public void StopFreezeSound()
    {
        freeze.Stop();
    }
    public void StopWindSound()
    {
        wind.Stop();
    }
    public void StopLightningSound()
    {
        lightning.Stop();
    }
    public void StopShieldSound()
    {
        shield.Stop();
    }
}
