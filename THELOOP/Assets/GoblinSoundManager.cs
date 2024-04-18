using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinSoundManager : MonoBehaviour
{
    [Header("Goblin")]
    public AudioSource goblinWalk;
    public AudioSource goblinLaugh;
    public AudioSource goblinAttack;
    public AudioSource goblinDeath;
    public AudioSource goblinScream;

    public void PlayGoblinWalk() { goblinWalk.Play(); }
    public void PlayGoblinAttack() { goblinAttack.Play(); }
    public void PlayGoblinDeath() { goblinDeath.Play(); }
    public void PlayGoblinScream() { goblinScream.Play(); }
    public void PlayGoblinLaugh() { goblinLaugh.Play(); }
}
