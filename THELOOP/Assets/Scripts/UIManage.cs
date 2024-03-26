using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManage : MonoBehaviour
{
    public GameObject damageText;
    public GameObject healthText;

    public Canvas canvas;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        
    }

    private void OnEnable()
    {
        CharacterEvents.tookDamaged += TookDamage;
        CharacterEvents.healed += Heal;
    }

    private void OnDisable()
    {
        CharacterEvents.tookDamaged -= TookDamage;
        CharacterEvents.healed -= Heal;
    }

    public void TookDamage(GameObject character, int damage)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text text = Instantiate(damageText, spawnPosition, Quaternion.identity, canvas.transform).GetComponent<TMP_Text>();

        text.text = damage.ToString();
    }

    public void Heal(GameObject character, int heal)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text text = Instantiate(healthText, spawnPosition, Quaternion.identity, canvas.transform).GetComponent<TMP_Text>();

        text.text = heal.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
