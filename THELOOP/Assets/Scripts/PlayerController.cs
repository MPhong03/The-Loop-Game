using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingEvents), typeof(DamageController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 20f;
    public float jumpForce = 40f;

    // Dash Field
    [Header("Dash variables")]
    [SerializeField]
    public bool canDash = true;
    [SerializeField]
    public bool isDashing;
    [SerializeField]
    public float dashPower = 20f;
    [SerializeField]
    public float dashTime = 0.2f;
    [SerializeField]
    public float dashCooldown = 1.0f;
    TrailRenderer trailRenderer;
    [SerializeField]
    public float dashGravity = 0f;
    [SerializeField]
    public float dashAttackTime = 0.75f;
    [SerializeField]
    private float dashAttackPower = 10f;
    private float normalGravity;
    private float waitTime;

    Vector2 moveInput;
    TouchingEvents touchingEvents;

    [SerializeField]
    private bool _isMoving = false;

    public float MoveSpeed
    {
        get
        {
            if (CanMove && !touchingEvents.IsWall)
            {
                return walkSpeed;
            }
            else
            {
                return 0;
            }
        }
    }

    public bool IsMoving {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationVariables.isMoving, value);
        }
    }

    public bool _isFacingRight = true;

    public bool IsFacingRight { get
        {
            return _isFacingRight;
        } 
        private set
        {
            if(_isFacingRight != value)
            {
                // Flip
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        } 
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationVariables.canMove);
        }
    }

    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationVariables.isAlive);
        }
    }

    public bool IsHit
    {
        get
        {
            return animator.GetBool(AnimationVariables.isHit);
        }
        private set
        {
            animator.SetBool(AnimationVariables.isHit, value);
        }
    }

    Rigidbody2D rb;
    Animator animator;
    DamageController damage;

    [Header("Pyro Blade Skill")]
    public GameObject skillPrefab;
    public bool skillStatusOne = false;
    private bool canUseSkill1 = true;

    [Header("Lightning Cloud Skill")]
    public GameObject lightningSkillPrefab;
    public bool skillStatusTwo = false;

    [Header("Shield Skill")]
    public GameObject shieldIcon;
    public float shieldTime = 7f;
    public bool skillStatusThree = false;

    public float skillCooldown = 7f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingEvents = GetComponent<TouchingEvents>();
        trailRenderer = GetComponent<TrailRenderer>();
        normalGravity = rb.gravityScale;
        canDash = true;
        damage = GetComponent<DamageController>();
    }

    private void Start()
    {
        if (FindObjectOfType<GlobalManager>() != null)
        {
            animator.runtimeAnimatorController = GlobalManager.Instance.GlobalAnimatorController;
            damage.Health = GlobalManager.Instance.health;
            if (GlobalManager.Instance.buffs.Count > 0)
            {
                foreach (var buff in GlobalManager.Instance.buffs)
                {
                    ApplyBuff(buff);
                }
            }

        }
    }


    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        rb.velocity = new Vector2(moveInput.x * MoveSpeed, rb.velocity.y);

        animator.SetFloat(AnimationVariables.airVelocity, rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;

            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }

        

    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        } 
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingEvents.IsTouch && CanMove)
        {
            animator.SetTrigger(AnimationVariables.jump);

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            Debug.Log("Dash conditions met, invoking Dash, Player: " + gameObject.activeInHierarchy + gameObject.activeSelf);
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        Debug.Log("Dash!");
        canDash = false;
        isDashing = true;
        damage.isInvincible = true;
        //float originalGravity = rb.gravityScale;
        //rb.gravityScale = 0;
        rb.velocity = new Vector2(transform.localScale.x * dashPower, 0);
        animator.SetBool(AnimationVariables.isDashing, isDashing);
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashTime);
        trailRenderer.emitting = false;
        //rb.gravityScale = originalGravity;
        isDashing = false;
        damage.isInvincible = false;
        animator.SetBool(AnimationVariables.isDashing, isDashing);
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private IEnumerator DashAttack()
    {
        Debug.Log("DashAttack!");
        canDash = false;
        isDashing = true;

        rb.velocity = new Vector2(transform.localScale.x * dashAttackPower, 0);
        
        yield return new WaitForSeconds(dashAttackTime);

        isDashing = false;
        canDash = true;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Attack!");
            animator.SetTrigger(AnimationVariables.attack);
        }
    }

    public void OnDashAttack()
    {
        StartCoroutine(DashAttack());
    }

    public void OnSpecialSkill(InputAction.CallbackContext context)
    {
        if (context.performed && canUseSkill1)
        {
            if (skillStatusOne)
            {
                StartCoroutine(SpawnSkillPrefab());
            }
            else if (skillStatusTwo)
            {
                StartCoroutine(SpawnLightningSkillPrefab());
            }
            else if (skillStatusThree)
            {
                StartCoroutine(ShieldSkill());
            }
        }
    }

    private IEnumerator SpawnSkillPrefab()
    {
        canUseSkill1 = false;
        Vector3 spawnDirection = _isFacingRight ? Vector2.right : Vector2.left;

        GameObject spawnedSkill = Instantiate(skillPrefab, transform.position, Quaternion.identity);
        spawnedSkill.GetComponent<FireballMovement>().Initialize(spawnDirection);

        yield return new WaitForSeconds(skillCooldown);
        canUseSkill1 = true;
    }

    private IEnumerator SpawnLightningSkillPrefab()
    {
        canUseSkill1 = false;
        GameObject nearestEnemy = FindNearestEnemyWithTag("Enemies");

        if (nearestEnemy != null)
        {
            GameObject lightning = Instantiate(lightningSkillPrefab, nearestEnemy.transform.position, Quaternion.identity);
            
        }

        yield return new WaitForSeconds(skillCooldown);
        canUseSkill1 = true;
    }

    private IEnumerator ShieldSkill()
    {
        damage.InvicibleBuff = true;

        shieldIcon.SetActive(true);

        Debug.Log("Shield activated. Player is invincible!");

        yield return new WaitForSeconds(shieldTime);

        damage.InvicibleBuff = false;

        shieldIcon.SetActive(false);

        Debug.Log("Shield deactivated. Player is no longer invincible.");

        yield return new WaitForSeconds(skillCooldown);
    }
    private GameObject FindNearestEnemyWithTag(string tag)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);
        GameObject nearest = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, currentPosition);
            if (distance < minDistance)
            {
                nearest = enemy;
                minDistance = distance;
            }
        }

        return nearest;
    }
    public void ApplyBuff(Buff buff)
    {
        switch (buff.tag)
        {
            // TODO: Thêm các case cho các buff khác
            case 1:
                skillStatusOne = true;
                skillStatusTwo = false;
                skillStatusThree = false;
                break;
            case 2:
            case 3:
                skillStatusTwo = true;
                skillStatusOne = false;
                skillStatusThree= false;
                break;
            case 4:
            case 5:
                skillStatusThree = true;
                skillStatusOne = false;
                skillStatusTwo = false;
                break;
            default:
                Debug.Log("Buff tag not recognized");
                break;
        }
    }
}
