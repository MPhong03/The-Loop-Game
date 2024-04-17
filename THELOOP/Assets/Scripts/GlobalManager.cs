using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance { get; set; }
    public RuntimeAnimatorController GlobalAnimatorController;
    public int health = 100;
    public List<Buff> buffs = new List<Buff>();

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
    }

    [System.Serializable]
    public class BuffList
    {
        public List<Buff> Buffs;
    }
}
