using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSoundManager : MonoBehaviour
{
    [Header("Boss")]
    public AudioSource attack1;
    public AudioSource attack2;
    public AudioSource takeHit;
    public AudioSource laugh;
    public AudioSource death;
    public AudioSource walk;

    public void PlayAttack1() { attack1.Play(); }
    public void PlayAttack2() { attack1.Play(); }
    public void PlayTakeHit() { takeHit.Play(); }
    public void PlayLaugh() { laugh.Play(); }
    public void PlayDeath() { death.Play(); }
    public void PlayWalk() { walk.Play(); }
}
