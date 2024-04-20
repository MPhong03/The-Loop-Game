using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSource;

    public AudioClip mainMenuMusic;
    public AudioClip startPointMusic;
    public AudioClip bossSceneMusic;
    public AudioClip restSceneMusic;
    public AudioClip[] forestMusic;

    private AudioClip currentMusic;
    private Coroutine fadeOutCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        switch (sceneName)
        {
            case "MainMenu":
                PlayMusic(mainMenuMusic);
                break;
            case "StartPoint":
                PlayMusic(startPointMusic);
                break;
            case "BossScene":
                PlayMusic(bossSceneMusic);
                break;
            case "RestPoint":
                PlayMusic(restSceneMusic);
                break;
            default:
                if (sceneName.StartsWith("Forest"))
                {
                    int forestIndex = int.Parse(sceneName.Substring(6));
                    if (forestIndex >= 1 && forestIndex <= 15)
                    {
                        int randomIndex = Random.Range(0, forestMusic.Length);
                        AudioClip selectedClip = forestMusic[randomIndex];
                        PlayMusic(selectedClip);
                    }
                }
                break;
        }
    }

    private void PlayMusic(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public void StopMusicSoftly(float fadeOutDuration)
    {
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
        }

        fadeOutCoroutine = StartCoroutine(FadeOutMusic(fadeOutDuration));
    }

    private IEnumerator FadeOutMusic(float fadeOutDuration)
    {
        float startVolume = audioSource.volume;
        float timer = 0f;

        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;
            float volume = Mathf.Lerp(startVolume, 0f, timer / fadeOutDuration);
            audioSource.volume = volume;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
