using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuffItemUI : MonoBehaviour, IPointerClickHandler
{
    public TMP_Text titleText;
    public TMP_Text descriptionText;

    private Buff currentBuff;
    private PlayerController playerController;

    public BuffPanelUI buffPanel;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        buffPanel = FindAnyObjectByType<BuffPanelUI>();
    }

    public void Setup(Buff buff)
    {
        currentBuff = buff;
        titleText.text = buff.title;
        descriptionText.text = buff.description;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Buff selected: " + currentBuff.title);
        // Apply buff logic here
        playerController.ApplyBuff(currentBuff);
        StartCoroutine(buffPanel.FadeOut());
    }
}
