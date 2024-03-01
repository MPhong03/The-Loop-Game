using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingEvents : MonoBehaviour
{
    public ContactFilter2D contactFilter;
    public float groundDistance = 0.05f;

    CapsuleCollider2D touchingCollider;
    Animator animator;
    //Rigidbody2D rb;

    RaycastHit2D[] groundHits = new RaycastHit2D[5];

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
    }
}
