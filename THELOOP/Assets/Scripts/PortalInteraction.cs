using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;

public class PortalInteraction : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject interactionUI;
    public LoadingScreenController loadingScreenController;
    private bool isPlayerNear = false;
    private bool canInteract = false;
    private List<int> randomList = new List<int>();
    public int min;
    public int max;

    [Header("Boss scene transition's condition")]
    public int bossSceneCondition = 4;
    private int restSceneID = 18;
    private int bossSceneID = 17;
    // Start is called before the first frame update
    void Start()
    {
        if (playerObject == null)
        {
            Debug.LogError("There's no player gameobject");
            return;
        }
        if (loadingScreenController == null)
        {
            Debug.LogError("Loading Screen Controller is not assigned!");
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
            interactionUI.SetActive(false);

            GlobalManager.Instance.sceneTransitionCount++;

            if (GlobalManager.Instance.sceneTransitionCount >= bossSceneCondition && GlobalManager.Instance.isFinishNormal == false)
            {
                GlobalManager.Instance.isFinishNormal = true;
                Debug.Log("Loading special scene: " + restSceneID);
                loadingScreenController.LoadScene(restSceneID);
            }
            else if (GlobalManager.Instance.isFinishNormal)
            {
                loadingScreenController.LoadScene(bossSceneID);
            }
            else
            {
                int number = Random.Range(min, max + 1);
                Debug.Log("Loading normal scene: " + number);
                loadingScreenController.LoadScene(number);
            }
        }
    }
}
