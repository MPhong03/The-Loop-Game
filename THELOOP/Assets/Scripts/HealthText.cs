using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    public Vector3 speed = new Vector3(0, 80, 0);
    private Color startColor;

    public float fadeTime = 1f;

    RectTransform rectTransform;
    TextMeshProUGUI textMeshPro;

    private float elapsed = 0f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        startColor = textMeshPro.color;
    }

    void Update()
    {
        rectTransform.position += speed * Time.deltaTime;

        elapsed += Time.deltaTime;

        if (elapsed < fadeTime )
        {
            float fade = startColor.a * (1 - (elapsed / fadeTime));
            textMeshPro.color = new Color(startColor.r, startColor.g, startColor.b, fade);
        } 
        else
        {
            Destroy(gameObject);
        }
    }
}
