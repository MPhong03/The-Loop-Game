using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreenController : MonoBehaviour
{
    public GameObject loadingScreen;
    public Image loadingBarFill;

    public void LoadScene(int sceneID)
    {
        StartCoroutine(LoadSceneAsync(sceneID));
    }

    IEnumerator LoadSceneAsync(int sceneID)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progessValue = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBarFill.fillAmount = progessValue;
            yield return null;
        }
    }
}
