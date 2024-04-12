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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingEvents = GetComponent<TouchingEvents>();
        trailRenderer = GetComponent<TrailRenderer>();
        normalGravity = rb.gravityScale;
        canDash = true;
        damage = GetComponent<DamageController>();

        if (FindObjectOfType<GlobalManager>() != null)
        {
            animator.runtimeAnimatorController = GlobalManager.Instance.GlobalAnimatorController;
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
        //float originalGravity = rb.gravityScale;
        //rb.gravityScale = 0;
        rb.velocity = new Vector2(transform.localScale.x * dashPower, 0);
        animator.SetBool(AnimationVariables.isDashing, isDashing);
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashTime);
        trailRenderer.emitting = false;
        //rb.gravityScale = originalGravity;
        isDashing = false;
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
            StartCoroutine(DashAttack());
        }
    }


}
