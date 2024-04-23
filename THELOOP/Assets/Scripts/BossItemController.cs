using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BossItemController : MonoBehaviour
{
    public GameObject popupButton;
    public GameObject blackoutPanel;
    public TMP_Text blackoutText;
    public string[] dialogueLines;
    public float fadeDuration = 5f; // Thời gian để các dòng chữ dần dần ẩn đi
    public float textDelay = 3f; // Độ trễ giữa các dòng chữ
    private Collider2D col;
    private bool playerInRange = false;

    LoadingScreenController loadingScreenController;
    private float delayBetweenCharacters = 0.05f;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        loadingScreenController = FindAnyObjectByType<LoadingScreenController>();
    }

    private void Update()
    {
        // Kiểm tra nếu người chơi ở gần và nhấn phím F
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(ShowDialogue());
        }
    }

    private IEnumerator ShowDialogue()
    {
        popupButton.SetActive(false);

        // Tạo một TMP_Text động để hiển thị dòng chữ trong blackout panel
        TMP_Text dynamicText = Instantiate(blackoutText, blackoutPanel.transform);
        dynamicText.text = ""; // Xóa nội dung cũ của dynamicText

        // Hiển thị blackout panel và các dòng chữ dần dần
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            // Tính toán độ mờ và độ hiển thị dần dần
            float alpha = Mathf.Clamp01(timer / fadeDuration);

            // Cập nhật độ mờ và độ hiển thị của blackout panel
            SetAlphaForBlackoutPanelAndText(alpha, dynamicText);

            yield return null;
        }

        // Hiện từng dòng chữ với hiệu ứng type writing
        for (int i = 0; i < dialogueLines.Length; i++)
        {
            string currentLine = dialogueLines[i];
            dynamicText.text = ""; // Xóa nội dung cũ của dynamicText

            // Hiển thị từng ký tự trong dòng chữ với hiệu ứng type writing
            for (int j = 0; j < currentLine.Length; j++)
            {
                dynamicText.text += currentLine[j];
                yield return new WaitForSeconds(delayBetweenCharacters);
            }

            // Đợi một khoảng thời gian trước khi hiển thị dòng chữ tiếp theo
            yield return new WaitForSeconds(textDelay);
        }

        // Ẩn blackout panel và các dòng chữ dần dần
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            // Tính toán độ mờ và độ hiển thị dần dần
            float alpha = Mathf.Clamp01(1f - timer / fadeDuration);

            // Cập nhật độ mờ và độ hiển thị của blackout panel
            SetAlphaForBlackoutPanelAndText(alpha, dynamicText);

            yield return null;
        }

        // Reset player state
        ResetPlayerState();

        // Restart game
        RestartGame();

        // Destroy dynamicText sau khi hoàn thành
        Destroy(dynamicText.gameObject);
    }

    private void SetAlphaForBlackoutPanelAndText(float alpha, TMP_Text text)
    {
        // Cập nhật độ mờ và độ hiển thị của blackout panel
        CanvasGroup canvasGroup = blackoutPanel.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = alpha;
        }

        // Cập nhật độ mờ và độ hiển thị của dòng chữ trong blackout panel
        text.alpha = alpha;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            popupButton.SetActive(true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            popupButton.SetActive(false);
            playerInRange = false;
        }
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
    }

    private void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
