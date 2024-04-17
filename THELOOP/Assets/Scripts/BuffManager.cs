using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    [SerializeField]
    private List<Buff> availableBuffs;
    [SerializeField]
    private BuffPanelUI buffPanelUI;
    [SerializeField] 
    private GameObject buffActivatorPrefab;

    private void Awake()
    {
        FindObjectOfType<SpawnController>().OnSpawnCompleted += HandleSpawnCompleted;
    }

    private void HandleSpawnCompleted()
    {
        SpawnBuffActivator();
    }

    public void ShowBuffPanel()
    {
        List<Buff> buffsToDisplay = PickRandomBuffs(3);
        buffPanelUI.Setup(buffsToDisplay);
    }

    private void SpawnBuffActivator()
    {
        if (buffActivatorPrefab != null)
        {
            Instantiate(buffActivatorPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("BuffActivator Prefab is not set.");
        }
    }

    private List<Buff> PickRandomBuffs(int count)
    {
        List<Buff> pickedBuffs = new List<Buff>();
        List<Buff> tempBuffs = new List<Buff>(availableBuffs); 

        for (int i = 0; i < count; i++)
        {
            if (tempBuffs.Count == 0)
            {
                break;
            }
            int randIndex = Random.Range(0, tempBuffs.Count);
            pickedBuffs.Add(tempBuffs[randIndex]);
            tempBuffs.RemoveAt(randIndex);
        }

        return pickedBuffs;
    }

}
