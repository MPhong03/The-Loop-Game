using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance { get; set; }
    public RuntimeAnimatorController GlobalAnimatorController;
    public int health = 100;
    public List<Buff> buffs = new List<Buff>();
    public int sceneTransitionCount = -1; // Not include Start Point
    public bool isFinishNormal = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void UpdatePlayerHealth(int currentHealth)
    {
        health = currentHealth;
    }

    public void SavePlayerState()
    {
        PlayerPrefs.SetInt("Health", health);
        PlayerPrefs.SetString("Buffs", JsonUtility.ToJson(new BuffList { Buffs = buffs }));
        PlayerPrefs.SetInt("SceneTransitions", sceneTransitionCount);
        PlayerPrefs.SetInt("FinishNormal", isFinishNormal ? 1 : 0);
        PlayerPrefs.Save();

    }

    public void LoadPlayerState()
    {
        health = PlayerPrefs.GetInt("Health", 100);
        string buffsJson = PlayerPrefs.GetString("Buffs", "{}");
        BuffList loadedBuffs = JsonUtility.FromJson<BuffList>(buffsJson);
        if (loadedBuffs != null && loadedBuffs.Buffs != null)
        {
            buffs = loadedBuffs.Buffs;
        }
        sceneTransitionCount = PlayerPrefs.GetInt("SceneTransitions", 0);
        isFinishNormal = PlayerPrefs.GetInt("FinishNormal", 0) != 0;
    }

    [System.Serializable]
    public class BuffList
    {
        public List<Buff> Buffs;
    }
}
