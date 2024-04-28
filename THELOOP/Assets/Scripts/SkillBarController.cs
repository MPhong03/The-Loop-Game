using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBarController : MonoBehaviour
{
    public Image imageCooldown;
    public bool isSpecialSkill = true;

    private SkillCooldown skillCooldown;
    private SkillCooldown dashCooldown;
    private PlayerController controller;

    private void Start()
    {
        controller = FindObjectOfType<PlayerController>();
        skillCooldown = new SkillCooldown { cooldownTime = controller.skillCooldown + controller.skillTime };
        dashCooldown = new SkillCooldown { cooldownTime = controller.dashCooldown + controller.dashTime };
    }

    void Update()
    {
        if (isSpecialSkill)
        {
            UpdateCooldown(ref skillCooldown, controller.canUseSkill);
        }
        else
        {
            UpdateCooldown(ref dashCooldown, controller.canDash);
        }
    }
    private void UpdateCooldown(ref SkillCooldown cooldown, bool canUseSkill)
    {
        if (!canUseSkill && !cooldown.isOnCooldown)
        {
            cooldown.isOnCooldown = true;
            Debug.Log("Skill is on cooldown: " + cooldown.cooldownTime);
        }

        if (cooldown.isOnCooldown)
        {
            imageCooldown.fillAmount += 1 / cooldown.cooldownTime * Time.deltaTime;
            if (imageCooldown.fillAmount >= 1)
            {
                imageCooldown.fillAmount = 0;
                if (isSpecialSkill)
                {
                    Debug.Log("Skill cooldown finished");
                }
                else
                {
                    Debug.Log("Dash cooldown finished");
                }
                cooldown.isOnCooldown = false;
            }
        }
    }

    private struct SkillCooldown
    {
        public float cooldownTime;
        public bool isOnCooldown;
    }
}
