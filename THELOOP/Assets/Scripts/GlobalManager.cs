using Unity.VisualScripting;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance { get; set; }
    public RuntimeAnimatorController GlobalAnimatorController;
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
}
