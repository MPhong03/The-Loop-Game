using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public TMP_Text buffTitle;
    public TMP_Text buffDescription;
    public GameObject buffItemDisplay;
    public TMP_Text noBuffMessage;
    private GlobalManager globalManager;

    LoadingScreenController loadingScreenController;

    void Start()
    {
        loadingScreenController = FindAnyObjectByType<LoadingScreenController>();

        globalManager = GlobalManager.Instance;
        pauseMenuUI.SetActive(false);
        noBuffMessage.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuUI.activeInHierarchy)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;  // Resume game time
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;  // Pause game time
        DisplayLastBuff();
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        globalManager.SavePlayerState();
        loadingScreenController.LoadScene(0);
    }

    private void DisplayLastBuff()
    {
        if (globalManager.buffs.Count > 0)
        {
            Buff lastBuff = globalManager.buffs[globalManager.buffs.Count - 1];
            buffTitle.text = lastBuff.title;
            buffDescription.text = lastBuff.description;
            buffItemDisplay.SetActive(true);
            noBuffMessage.gameObject.SetActive(false);
        }
        else
        {
            buffItemDisplay.SetActive(false);
            noBuffMessage.gameObject.SetActive(true);
        }
    }
}
