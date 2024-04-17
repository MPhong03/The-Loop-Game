using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSkillController : MonoBehaviour
{
    public float duration = 2.0f;

    void Start()
    {
        Destroy(gameObject, duration);
    }

    void OnAnimationStart()
    {
        // Logic khi animation bắt đầu
    }

    void OnAnimationEnd()
    {
        Destroy(gameObject);
    }
}
