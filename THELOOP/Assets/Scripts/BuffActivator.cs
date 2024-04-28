using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffActivator : MonoBehaviour
{
    public GameObject interactUI;
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (interactUI.activeInHierarchy && Input.GetKeyDown(KeyCode.F))
        {
            audioSource.Play();
            FindObjectOfType<BuffManager>().ShowBuffPanel();
            interactUI.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        audioSource.Play();
    }
}
