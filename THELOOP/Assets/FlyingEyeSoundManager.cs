using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeSoundManager : MonoBehaviour
{
    [Header("Flying Eye")]
    public AudioSource fly;
    public AudioSource bite;
    public AudioSource scream;

    public void PlayFly() { fly.Play(); }
    public void PlayBite() { bite.Play(); }
    public void PlayScream() { scream.Play(); }
}
