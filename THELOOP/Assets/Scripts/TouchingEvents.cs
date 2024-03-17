using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TouchingEvents : MonoBehaviour
{
    public ContactFilter2D contactFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.2f;
    public float ceilingDistance = 0.05f;

    CapsuleCollider2D touchingCollider;
    Animator animator;
    //Rigidbody2D rb;

    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];

    [SerializeField]
    private bool _isTouch;
    public bool IsTouch { get
        {
            return _isTouch;
        }
        private set
        {
            _isTouch = value;
            animator.SetBool(AnimationVariables.isTouchTile, value);
        } 
    }

    [SerializeField]
    private bool _isWall;
    public bool IsWall
    {
        get
        {
            return _isWall;
        }
        private set
        {
            _isWall = value;
            animator.SetBool(AnimationVariables.isTouchWall, value);
        }
    }

    [SerializeField]
    private bool _isCeiling;
    private Vector2 wallDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    public bool IsCeiling
    {
        get
        {
            return _isCeiling;
        }
        private set
        {
            _isCeiling = value;
            animator.SetBool(AnimationVariables.isTouchCeiling, value);
        }
    }

    private void Awake()
    {
        touchingCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        IsTouch = touchingCollider.Cast(Vector2.down, contactFilter, groundHits, groundDistance) > 0;
        IsWall = touchingCollider.Cast(wallDirection, contactFilter, wallHits, wallDistance) > 0;
        IsCeiling = touchingCollider.Cast(Vector2.up, contactFilter, ceilingHits, ceilingDistance) > 0;
    }
}
