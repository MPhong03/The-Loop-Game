using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskBoardController : MonoBehaviour
{
    public GameObject letterUI;
    public GameObject pressFText;

    private bool isLetterVisible = false;
    private bool isPlayerNearby = false;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isLetterVisible)
            {
                HideLetter();
            }
            else if (isPlayerNearby)
            {
                ShowLetter();
            }
        }
    }

    void ShowLetter()
    {
        audioSource.Play();
        letterUI.SetActive(true);
        isLetterVisible = true;
    }

    void HideLetter()
    {
        audioSource.Play();
        letterUI.SetActive(false);
        isLetterVisible = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            pressFText.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            pressFText.SetActive(false);
        }
    }
}
