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

    public RuntimeAnimatorController sword;
    public RuntimeAnimatorController spear;
    public RuntimeAnimatorController axe;

    public int currentWeaponFlag = 1;

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

    private void Start()
    {
        LoadPlayerState();
    }

    private void OnApplicationQuit()
    {
        SavePlayerState();
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
        PlayerPrefs.SetInt("CurrentWeaponFlag", currentWeaponFlag);
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

        int weaponFlag = PlayerPrefs.GetInt("CurrentWeaponFlag", 1); // Sword is default
        ApplyWeaponController(weaponFlag);
    }

    private void ApplyWeaponController(int flag)
    {
        switch (flag)
        {
            case 1:
                GlobalAnimatorController = sword;
                break;
            case 2:
                GlobalAnimatorController = spear;
                break;
            case 3:
                GlobalAnimatorController = axe;
                break;
            default:
                GlobalAnimatorController = sword;
                break;
        }
    }

    public void ChangeWeapon(int weaponType)
    {
        currentWeaponFlag = weaponType;
        ApplyWeaponController(currentWeaponFlag);
    }

    [System.Serializable]
    public class BuffList
    {
        public List<Buff> Buffs;
    }
}
