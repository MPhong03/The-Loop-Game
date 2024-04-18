using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    public int healamount = 15;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageController damageController = collision.GetComponent<DamageController>();

        if (damageController)
        {
            damageController.Heal(healamount);

            audioSource.Play();

            GetComponent<Collider2D>().enabled = false;
            GetComponent<Renderer>().enabled = false;

            Destroy(gameObject, audioSource.clip.length);
        }
    }
}
