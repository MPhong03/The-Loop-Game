using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public int attackDamage = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageController damage = collision.GetComponent<DamageController>();

        if (damage != null)
        {
            bool hit = damage.Hit(attackDamage);
            if (hit)
            {
                Debug.Log("Taked hit!");
            }
            
        }
    }
}
