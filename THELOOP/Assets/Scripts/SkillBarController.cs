using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBarController : MonoBehaviour
{
    public Image imageCooldown;
    public float dashCooldown;
    bool isDashCooldown;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            isDashCooldown = true;
        }
        if (isDashCooldown)
        {
            imageCooldown.fillAmount += 1/dashCooldown*Time.deltaTime;
            if(imageCooldown.fillAmount >= 1)
            {
                imageCooldown.fillAmount = 0;
                isDashCooldown=false;
            }
        }
    }
}
