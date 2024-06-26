using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Slider slider;

    DamageController damageController;
    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null )
        {
            Debug.Log("No Player!");
        }

        damageController = player.GetComponent<DamageController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        slider.value = CalculateSlider(damageController.Health, damageController.MaxHealth);
    }

    private void OnEnable()
    {
        damageController.healthChanged.AddListener(OnPlayerHealthChanged);
    }

    private void OnDisable()
    {
        damageController.healthChanged.RemoveListener(OnPlayerHealthChanged);
    }

    private float CalculateSlider(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    }

    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        slider.value = CalculateSlider(newHealth, maxHealth);
    }
}
