using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Slider slider;
    public GameObject boss;

    DamageController damageController;
    private void Awake()
    {
        if (boss == null)
        {
            Debug.Log("No Boss!");
        }

        damageController = boss.GetComponent<DamageController>();
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
