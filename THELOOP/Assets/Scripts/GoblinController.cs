using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingEvents))]
public class GoblinScript : MonoBehaviour
{
    public float walkSpeed = 3f;
    Rigidbody2D rb;
    TouchingEvents touchingEvents;
    public enum WalkDirection { Right, Left };
    private WalkDirection _walkDirection;
    private Vector2 walkDirectionVector;
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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingEvents = GetComponent<TouchingEvents>();
    }
    private void FixedUpdate()
    {
        //if (touchingEvents)
        rb.velocity = new Vector2(walkSpeed * Vector2.right.x, rb.velocity.y);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
