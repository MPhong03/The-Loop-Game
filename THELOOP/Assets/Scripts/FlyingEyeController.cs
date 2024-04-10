using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlyingEyeController : MonoBehaviour
{
    public AIPath aiPath;

    public DetectionZone zone;

    Rigidbody2D rb;
    Animator animator;

    public bool _hasTarget = false;

    public bool HasTarget
    {
        get
        {
            return _hasTarget;
        }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationVariables.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationVariables.canMove);
        }
    }

    public float AttackCoolDown
    {
        get
        {
            return animator.GetFloat(AnimationVariables.attackCoolDown);
        }
        private set
        {
            animator.SetFloat(AnimationVariables.attackCoolDown, Mathf.Max(value, 0));
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (aiPath.desiredVelocity.x <= -0.01)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        HasTarget = zone.detectedCols.Count > 0;
        if (AttackCoolDown > 0)
        {
            AttackCoolDown -= Time.deltaTime;
        }
    }
}
