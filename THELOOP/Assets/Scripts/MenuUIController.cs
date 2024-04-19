using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIController : MonoBehaviour
{
    public Button playButton;
    public Button resumeButton;
    public LoadingScreenController loadingScreenController;

    void Start()
    {
        int savedSceneIndex = PlayerPrefs.GetInt("CurrentSceneIndex", 0);

        if (savedSceneIndex > 1)
        {
            playButton.gameObject.SetActive(false);
            resumeButton.gameObject.SetActive(true);
        }
        else
        {
            playButton.gameObject.SetActive(true);
            resumeButton.gameObject.SetActive(false);
        }

    }

    public void ResumeGame()
    {
        GlobalManager.Instance.LoadPlayerState();
        loadingScreenController.LoadScene(GlobalManager.Instance.sceneIndex);
    }

}
