using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingEvents), typeof(DetectionZone))]
public class GoblinScript : MonoBehaviour
{
    public DetectionZone zone;

    public float walkSpeed = 3f;
    public float walkStopRate = 0.05f;

    Rigidbody2D rb;
    TouchingEvents touchingEvents;
    Animator animator;

    public enum WalkDirection { Right, Left };
    private WalkDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;
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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingEvents = GetComponent<TouchingEvents>();
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (touchingEvents.IsWall && touchingEvents.IsTouch)
        {
            FlipDirection();
        }

        if (CanMove)
        {
            rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y); ;
        }   
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = zone.detectedCols.Count > 0;
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
}
