using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TriggerSideEvent : MonoBehaviour
{
    public float requiredTime = 10f; // Thời gian người chơi phải đứng ở game object để kích hoạt sự kiện
    public float fadeDuration = 2f; // Thời gian để màn hình đen dần dần hiển thị
    public string[] dialogueLines;
    public TMP_Text blackoutText;
    public GameObject blackoutPanel;
    public float textDelay = 3f;

    private bool playerInRange = false;
    private float currentTime = 0f;
    private bool hasTriggered = false;
    private float delayBetweenCharacters = 0.05f;

    LoadingScreenController loadingScreenController;
    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        loadingScreenController = FindAnyObjectByType<LoadingScreenController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            currentTime = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            currentTime = 0f;
        }
    }

    private void Update()
    {
        if (playerInRange && !hasTriggered)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= requiredTime)
            {
                hasTriggered = true;
                StartCoroutine(ShowDialogue());
            }
        }
    }

    private IEnumerator ShowDialogue()
    {
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
        loadingScreenController.LoadScene(0);
    }

    private void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
