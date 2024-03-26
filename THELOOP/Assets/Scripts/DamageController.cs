using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageController : MonoBehaviour
{
    Animator animator;

    public UnityEvent<int, Vector2> damageHit;

    [SerializeField]
    private int _maxHealth = 100;
    public int MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }

    [SerializeField]
    private int _health = 100;
    public int Health
    {
        get { return _health; }
        set 
        { 
            _health = value;

            if(_health <= 0)
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField]
    private bool _isAlive = true;
    [SerializeField]
    private bool isInvincible = false;
    private float timeSinceHit = 0;

    public float invincibleTime = 0.2f;

    public bool IsAlive 
    { 
        get
        {
            return _isAlive;
        }
        private set
        {
            _isAlive = value;
            animator.SetBool(AnimationVariables.isAlive, value);
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

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isInvincible)
        {
            if (timeSinceHit > invincibleTime)
            {
                isInvincible = false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }
    }

    public bool Hit(int damage)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;

            IsHit = true;

            CharacterEvents.tookDamaged.Invoke(gameObject, damage);

            return true;
        }
        return false;
    }

    public void Heal(int healamount)
    {
        if (IsAlive)
        {
            int maxheal = Mathf.Max(MaxHealth - Health, 0);
            Health += Mathf.Min(maxheal, healamount);

            CharacterEvents.healed(gameObject, healamount);
        }
    }
}
