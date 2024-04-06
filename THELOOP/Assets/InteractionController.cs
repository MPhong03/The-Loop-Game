using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public GameObject[] interactableObjects;
    public GameObject playerObject;

    public float interactionDistance = 2f;
    public GameObject interactionUI;

    public RuntimeAnimatorController weapon1Controller;
    public RuntimeAnimatorController weapon2Controller;
    public RuntimeAnimatorController weapon3Controller;

    private RuntimeAnimatorController currentAnimatorController;
    private Animator playerAnimator;
    private bool canInteract = false;

    void Start()
    {
        if (playerObject != null)
        {
            playerAnimator = playerObject.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("Player object is not assigned to PlayerInteraction script!");
        }
    }

    void Update()
    {
        CheckDistance();
        HandleInput();
    }

    void CheckDistance()
    {
        foreach (GameObject obj in interactableObjects)
        {
            float distance = Vector2.Distance(transform.position, obj.transform.position);
            if (distance < interactionDistance)
            {
                canInteract = true;
                currentAnimatorController = obj.GetComponent<Animator>().runtimeAnimatorController;
                break;
            }
            else
            {
                canInteract = false;
                currentAnimatorController = null;
            }
        }

        // Display F UI
        interactionUI.SetActive(canInteract);
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.F) && canInteract)
        {
            // Change AnimationController
            if (currentAnimatorController != null)
            {
                if (currentAnimatorController == weapon1Controller)
                {
                    playerAnimator.runtimeAnimatorController = weapon1Controller;
                }
                else if (currentAnimatorController == weapon2Controller)
                {
                    playerAnimator.runtimeAnimatorController = weapon2Controller;
                }
                else if (currentAnimatorController == weapon3Controller)
                {
                    playerAnimator.runtimeAnimatorController = weapon3Controller;
                }
            }
        }
    }
}
