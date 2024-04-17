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
    private GameObject buffActivator;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        buffPanel = FindAnyObjectByType<BuffPanelUI>();
        buffActivator = GameObject.FindGameObjectWithTag("BuffNPC");
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

        if (buffPanel.portal != null)
            buffPanel.portal.SetActive(true);

        if (buffActivator != null)
            Destroy(buffActivator);

        if (FindObjectOfType<GlobalManager>() != null)
        {
            GlobalManager.Instance.buffs.Add(currentBuff);
        }

        Destroy(buffPanel.gameObject);
    }
}
