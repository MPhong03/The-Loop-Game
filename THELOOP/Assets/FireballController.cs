using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private PolygonCollider2D coll;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<PolygonCollider2D>();

        Destroy(gameObject, 10f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        rb.velocity = Vector2.zero;
        coll.enabled = false;

        animator.Play("Explode");

        Destroy(gameObject, 1f);
    }
}
