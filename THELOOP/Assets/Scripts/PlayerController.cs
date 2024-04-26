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
    PlayerSoundManager soundManager;

    [Header("Pyro Blade Skill")]
    public GameObject skillPrefab;
    public bool skillStatusOne = false;

    [Header("Lightning Cloud Skill")]
    public GameObject lightningSkillPrefab;
    public bool skillStatusTwo = false;

    [Header("Shield Skill")]
    public GameObject shieldIcon;
    public float shieldTime = 7f;
    public bool skillStatusThree = false;

    [Header("Heal Skill")]
    public bool skillStatusFour = false;

    [Header("Anemo Blade Skill")]
    public GameObject anemoPrefab;
    public bool skillStatusFive = false;

    [Header("Freeze Skill")]
    public GameObject freezeEffectPrefab;
    public float freezeTime = 5f;
    public bool skillStatusSix = false;

    public float skillCooldown = 7f;
    private bool canUseSkill1 = true;

    public ContactFilter2D contactFilter;
    private CapsuleCollider2D col;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingEvents = GetComponent<TouchingEvents>();
        trailRenderer = GetComponent<TrailRenderer>();
        normalGravity = rb.gravityScale;
        canDash = true;
        damage = GetComponent<DamageController>();
        soundManager = GetComponent<PlayerSoundManager>();
        col = GetComponent<CapsuleCollider2D>();
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

        if (!IsMoving || !touchingEvents.IsTouch)
        {
            soundManager.StopWalkSound();
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
            soundManager.PlayWalkSound();
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
            soundManager.PlayJumpSound();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    public void OnDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

            RaycastHit2D[] hits = new RaycastHit2D[1];

            col.Cast(Vector2.down, contactFilter, hits, 0.05f);

            int hitCount = col.Cast(Vector2.down, contactFilter, hits, 0.05f);

            for (int i = 0; i < hitCount; i++)
            {
                Debug.Log("Object hit: " + hits[i].collider.gameObject.name);
            }

            if (hitCount > 0 && hits[0].collider.CompareTag("Platform"))
            {
                Debug.Log("Success!");

                Physics2D.IgnoreCollision(col, hits[0].collider, true);

                StartCoroutine(ResetCollisionAfterDelay(hits[0].collider));
            }
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            Debug.Log("Dash conditions met, invoking Dash, Player: " + gameObject.activeInHierarchy + gameObject.activeSelf);
            soundManager.PlayDashSound();
            StartCoroutine(Dash());
        }
    }

    private IEnumerator ResetCollisionAfterDelay(Collider2D colliderToReset)
    {
        yield return new WaitForSeconds(0.5f);

        Physics2D.IgnoreCollision(col, colliderToReset, false);
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
                StartCoroutine(SpawnFireBladePrefab());
            }
            else if (skillStatusTwo)
            {
                StartCoroutine(SpawnLightningSkillPrefab());
            }
            else if (skillStatusThree)
            {
                StartCoroutine(ShieldSkill());
            }
            else if (skillStatusFour)
            {
                StartCoroutine(HealSkill());
            }
            else if (skillStatusFive)
            {
                StartCoroutine(SpawnAnemoBladePrefab());
            }
            else if (skillStatusSix)
            {
                StartCoroutine(FreezeEnemies());
            }
        }
    }

    // FIRE BLADE SKILL
    private IEnumerator SpawnFireBladePrefab()
    {
        canUseSkill1 = false;
        Vector3 spawnDirection = _isFacingRight ? Vector2.right : Vector2.left;

        GameObject spawnedSkill = Instantiate(skillPrefab, transform.position, Quaternion.identity);
        spawnedSkill.GetComponent<FireballMovement>().Initialize(spawnDirection);

        soundManager.PlayFireSound();

        yield return new WaitForSeconds(skillCooldown);
        canUseSkill1 = true;
    }

    // LIGHTNING SKILL
    private IEnumerator SpawnLightningSkillPrefab()
    {
        canUseSkill1 = false;
        GameObject nearestEnemy = FindNearestEnemyWithTag("Enemies");

        if (nearestEnemy != null)
        {
            GameObject lightning = Instantiate(lightningSkillPrefab, nearestEnemy.transform.position, Quaternion.identity);
            soundManager.PlayLightningSound();
        }

        yield return new WaitForSeconds(skillCooldown);
        canUseSkill1 = true;
    }

    // SHIELD SKILL
    private IEnumerator ShieldSkill()
    {
        canUseSkill1 = false;

        damage.InvicibleBuff = true;

        shieldIcon.SetActive(true);

        soundManager.PlayShieldSound();

        Debug.Log("Shield activated. Player is invincible!");

        yield return new WaitForSeconds(shieldTime);

        damage.InvicibleBuff = false;

        shieldIcon.SetActive(false);

        Debug.Log("Shield deactivated. Player is no longer invincible.");

        yield return new WaitForSeconds(skillCooldown);

        canUseSkill1 = true;
    }

    // HEAL SKILL
    private IEnumerator HealSkill()
    {
        canUseSkill1 = false;
        soundManager.PlayHealSound();
        damage.Heal(50);
        yield return new WaitForSeconds(skillCooldown);

        canUseSkill1 = true;
    }

    // ANEMO SKILL
    private IEnumerator SpawnAnemoBladePrefab()
    {
        canUseSkill1 = false;
        Vector3 spawnDirection = _isFacingRight ? Vector2.right : Vector2.left;

        GameObject spawnedSkill = Instantiate(anemoPrefab, transform.position, Quaternion.identity);
        spawnedSkill.GetComponent<FireballMovement>().Initialize(spawnDirection);

        soundManager.PlayWindSound();

        yield return new WaitForSeconds(skillCooldown);
        canUseSkill1 = true;
    }

    // FREEZE SKILL
    private IEnumerator FreezeEnemies()
    {
        canUseSkill1 = false;

        // Find all tag "Enemies"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemies");

        // Freeze all Enemies
        foreach (GameObject enemy in enemies)
        {
            // Kiểm tra xem enemy còn tồn tại không
            if (enemy != null)
            {
                Animator enemyAnimator = enemy.GetComponent<Animator>();
                Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();

                if (enemyAnimator != null)
                {
                    enemyAnimator.enabled = false;
                }

                if (enemyRb != null)
                {
                    enemyRb.constraints = RigidbodyConstraints2D.FreezeAll;
                    GameObject effect = Instantiate(freezeEffectPrefab, enemy.transform.position, Quaternion.identity);
                    Animator effectAnimator = effect.GetComponent<Animator>();
                    AnimatorStateInfo stateInfo = effectAnimator.GetCurrentAnimatorStateInfo(0);
                    float effectDuration = stateInfo.length;

                    Destroy(effect, effectDuration);
                }
            }
        }

        soundManager.PlayFreezeSound();

        // Freeze time
        yield return new WaitForSeconds(freezeTime);

        // Unfreeze
        foreach (GameObject enemy in enemies)
        {
            // Kiểm tra xem enemy còn tồn tại không
            if (enemy != null)
            {
                Animator enemyAnimator = enemy.GetComponent<Animator>();
                Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();

                if (enemyAnimator != null)
                {
                    enemyAnimator.enabled = true;
                }

                if (enemyRb != null)
                {
                    enemyRb.constraints = RigidbodyConstraints2D.None;
                    enemyRb.constraints = RigidbodyConstraints2D.FreezeRotation;
                }
            }
        }

        canUseSkill1 = true;
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
                UpdateSkillStatus(true, false, false, false, false, false);
                break;
            case 2:
                UpdateSkillStatus(false, false, false, true, false, false);
                break;
            case 3:
                UpdateSkillStatus(false, true, false, false, false, false);
                break;
            case 4:
                UpdateSkillStatus(false, false, false, false, false, true);
                break;
            case 5:
                UpdateSkillStatus(false, false, true, false, false, false);
                break;
            case 6:
                UpdateSkillStatus(false, false, false, false, true, false);
                break;
            default:
                Debug.Log("Buff tag not recognized");
                break;
        }
    }

    private void UpdateSkillStatus(bool one, bool two, bool three, bool four, bool five, bool six)
    {
        skillStatusOne = one;
        skillStatusTwo = two;
        skillStatusThree = three;
        skillStatusFour = four;
        skillStatusFive = five;
        skillStatusSix = six;
    }
}
