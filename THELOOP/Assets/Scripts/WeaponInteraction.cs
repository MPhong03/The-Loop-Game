using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject interactionUI;
    private bool isPlayerNear = false;

    public RuntimeAnimatorController objectAnimationController; 
    private bool canInteract = false;

    void Start()
    {
        if (playerObject == null)
        {
            Debug.LogError("There's no player gameobject");
            return;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            interactionUI.SetActive(true);
            canInteract = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            interactionUI.SetActive(false);
            canInteract = false;
        }
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F) && canInteract)
        {
            CheckAndChangePlayerAnimationController();
        }
    }

    void CheckAndChangePlayerAnimationController()
    {
        Animator playerAnimator = playerObject.GetComponent<Animator>();
        if (playerAnimator != null && objectAnimationController != null)
        {
            if (playerAnimator.runtimeAnimatorController != objectAnimationController)
            {
                playerAnimator.runtimeAnimatorController = objectAnimationController;

                GlobalManager.Instance.GlobalAnimatorController = objectAnimationController;
            }
        }
    }

    public void SetAnimationController(RuntimeAnimatorController controller)
    {
        objectAnimationController = controller;
    }
}
