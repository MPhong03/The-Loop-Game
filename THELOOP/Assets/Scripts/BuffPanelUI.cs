using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BuffPanelUI : MonoBehaviour
{
    public BuffItemUI buffItemPrefab;
    public Transform buffContainer;
    public CanvasGroup canvasGroup;
    public float fadeDuration = 0.5f;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        gameObject.SetActive(false);
        SetupHorizontalLayoutGroup();
    }

    void SetupHorizontalLayoutGroup()
    {
        HorizontalLayoutGroup layoutGroup = buffContainer.gameObject.GetComponent<HorizontalLayoutGroup>();
        if (layoutGroup == null)
        {
            layoutGroup = buffContainer.gameObject.AddComponent<HorizontalLayoutGroup>();
        }

        layoutGroup.childAlignment = TextAnchor.MiddleCenter;
    }

    public void Setup(List<Buff> buffs)
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 0;
        foreach (Transform child in buffContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Buff buff in buffs)
        {
            BuffItemUI item = Instantiate(buffItemPrefab, buffContainer);
            item.Setup(buff);
        }
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = elapsedTime / fadeDuration;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;
    }

    public IEnumerator FadeOut()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = 1 - (elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0;
        gameObject.SetActive(false);  // Deactivate after fading out
    }
}
