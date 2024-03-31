using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackSmithController : MonoBehaviour
{
    public GameObject shopPanel;
    public GameObject promptMessage;
    private bool isPlayerNear = false;

    Collider2D col;
    Rigidbody2D rb;
    Animator animator;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            shopPanel.SetActive(true);
            promptMessage.SetActive(false);
        } 
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isPlayerNear = true;
        promptMessage.SetActive(true);
        Debug.Log("Player enter!");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isPlayerNear = false;
        promptMessage.SetActive(false);
        Debug.Log("Player exit!");
    }
}
