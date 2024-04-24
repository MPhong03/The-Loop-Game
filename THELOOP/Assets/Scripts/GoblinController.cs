using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingEvents), typeof(DetectionZone))]
public class GoblinScript : MonoBehaviour
{
    public DetectionZone zone;
    public DetectionZone groundZone;

    public float walkSpeed = 3f;
    public float walkStopRate = 0.05f;
    public float jumpForce = 10f;

    Rigidbody2D rb;
    TouchingEvents touchingEvents;
    Animator animator;
    CapsuleCollider2D col;

    public enum WalkDirection { Right, Left };
    private WalkDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;
    private Transform player;
    private Vector2 playerLastPosition;

    public ContactFilter2D contactFilter;
    public float playerDistanceThreshold = 6f;

    public WalkDirection walkDirection
    {
        get { return _walkDirection; }
        set { 
            if(_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
                if(value == WalkDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }
                else if(value == WalkDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }

            _walkDirection = value; }
    }

    public bool _hasTarget = false;

    public bool HasTarget { get
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
        touchingEvents = GetComponent<TouchingEvents>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerLastPosition = player.position;
        col = GetComponent<CapsuleCollider2D>();
    }
    private void FixedUpdate()
    {
        if (player != null && touchingEvents.IsTouch && Mathf.Abs(player.position.x - transform.position.x) < 2.5f)
        {
            float yDistance = player.position.y - transform.position.y;

            if (yDistance < 0 && Mathf.Abs(yDistance) >= playerDistanceThreshold)
            {
                RaycastHit2D[] hits = new RaycastHit2D[1];

                col.Cast(Vector2.down, contactFilter, hits, 0.05f);

                int hitCount = col.Cast(Vector2.down, contactFilter, hits, 0.05f);

                if (hitCount > 0 && hits[0].collider.CompareTag("Platform"))
                {

                    Physics2D.IgnoreCollision(col, hits[0].collider, true);

                    StartCoroutine(ResetCollisionAfterDelay(hits[0].collider));
                }
            }
            else if (yDistance > 0 && Mathf.Abs(yDistance) >= playerDistanceThreshold)
            {
                // Kiểm tra nếu enemy chưa đạt đến độ cao tối đa của nhảy
                if (transform.position.y < player.position.y + 6f)
                {
                    // Thực hiện hành động nhảy lên
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                }
                else
                {
                    // Nếu đã đạt đến độ cao tối đa, giảm tốc độ lên hoặc ngừng di chuyển lên
                    rb.velocity = new Vector2(rb.velocity.x, 0f);
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (player != null && CanMove)
        {
            // Tính toán hướng vector từ Goblin tới người chơi trên trục x
            Vector2 direction = new Vector2(player.position.x - transform.position.x, 0f).normalized;
            // Di chuyển Goblin theo hướng này, giữ nguyên thành phần vận tốc y để cho phép trọng lực tác động
            rb.velocity = new Vector2(direction.x * walkSpeed, rb.velocity.y);

            // Flip hình ảnh nếu cần
            if (direction.x > 0)
            {
                // Đang nhìn về bên phải
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                // Đang nhìn về bên trái
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }

        HasTarget = zone.detectedCols.Count > 0;
        if (AttackCoolDown > 0)
        {
            AttackCoolDown -= Time.deltaTime;
        }
        
    }

    private IEnumerator ResetCollisionAfterDelay(Collider2D colliderToReset)
    {
        yield return new WaitForSeconds(1f);

        Physics2D.IgnoreCollision(col, colliderToReset, false);
    }

    private void FlipDirection()
    {
        if(walkDirection == WalkDirection.Right)
        {
            walkDirection = WalkDirection.Left;
        }
        else if (walkDirection == WalkDirection.Left)
        {
            walkDirection = WalkDirection.Right;
        }
        else
        {
            Debug.Log("Undefined direction");
        }
    }

    public void OnNoGround()
    {
        if (touchingEvents.IsTouch)
        {
            FlipDirection();
        }
    }
}
