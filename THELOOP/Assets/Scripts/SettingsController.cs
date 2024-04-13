using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SettingsController : MonoBehaviour
{
    public AudioMixer audio;
    public void SetVolume (float volume)
    {
        audio.SetFloat("Volume", volume);
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel (qualityIndex);
    }

    public void SaveChanges()
    {
        SaveChanges();
        SceneManager.LoadScene("MainMenu"); 
    }
}
