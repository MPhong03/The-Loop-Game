using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;

public class PortalInteraction : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject interactionUI;
    private bool isPlayerNear = false;
    private bool canInteract = false;
    private List<int> randomList = new List<int>();
    // Start is called before the first frame update
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
            int number = Random.Range(1,6);
            Debug.Log(number);
            SceneManager.LoadScene(number);
        }
    }
}
