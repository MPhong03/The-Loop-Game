using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    public int healamount = 15;
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageController damageController = collision.GetComponent<DamageController>();

        if (damageController)
        {
            damageController.Heal(healamount);

            Destroy(gameObject);
        }
    }
}
