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
    public TMP_Text buffStats;
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

        buffTitle.text = string.Empty;
        buffDescription.text = string.Empty;
        buffStats.text = string.Empty;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
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
        DisplayPlayerBuff();
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        globalManager.SavePlayerState();
        pauseMenuUI.SetActive(false);
        loadingScreenController.LoadScene(0);
    }

    private void DisplayPlayerBuff()
    {
        List<Buff> eligibleBuffs = new List<Buff>();

        if (globalManager.buffs.Count > 0)
        {
            foreach (Buff buff in globalManager.buffs)
            {
                if (buff.tag <= 6)
                {
                    eligibleBuffs.Add(buff);
                }
            }

            if (eligibleBuffs.Count > 0)
            {
                // Lấy buff cuối cùng có tag <= 6
                Buff lastBuff = eligibleBuffs[eligibleBuffs.Count - 1];
                buffTitle.text = lastBuff.title;
                buffDescription.text = lastBuff.description;
            }

            // Hiển thị các buff có tag > 6
            string buffStatsText = "";
            foreach (Buff buff in globalManager.buffs)
            {
                if (buff.tag > 6)
                {
                    buffStatsText += buff.description + "\n";
                }
            }
            buffStats.text = buffStatsText;

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
