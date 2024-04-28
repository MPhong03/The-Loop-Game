using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDeathHandler : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Button restartButton;
    public Button mainMenuButton;

    LoadingScreenController loadingScreenController;

    private void Start()
    {
        loadingScreenController = FindAnyObjectByType<LoadingScreenController>();

        gameOverPanel.SetActive(false);

        restartButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    public void OnPlayerDeath()
    {
        GameObject[] damageTextObjects = GameObject.FindGameObjectsWithTag("DamageText");

        foreach (GameObject obj in damageTextObjects)
        {
            obj.SetActive(false);
        }

        gameOverPanel.SetActive(true);
        Time.timeScale = 0;

        ResetPlayerState();
    }

    private void ResetPlayerState()
    {
        GlobalManager.Instance.UpdatePlayerHealth(100);
        GlobalManager.Instance.buffs.Clear();
        GlobalManager.Instance.currentWeaponFlag = 1;
        GlobalManager.Instance.ChangeWeapon(GlobalManager.Instance.currentWeaponFlag);
        GlobalManager.Instance.sceneIndex = 1;
        GlobalManager.Instance.sceneTransitionCount = -1;
        GlobalManager.Instance.isFinishNormal = false;
    }

    private void RestartGame()
    {
        ClearPlayerPrefs();
        Time.timeScale = 1;
        loadingScreenController.LoadScene(1);

        gameOverPanel.SetActive(false);
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1;
        loadingScreenController.LoadScene(0);

        gameOverPanel.SetActive(false);
    }

    private void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
