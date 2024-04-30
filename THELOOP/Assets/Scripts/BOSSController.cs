using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSSController : MonoBehaviour
{
    public DetectionZone zone;
    public GameObject tornadoPrefab;
    public Transform firePoint;
    public GameObject unknowMagic;

    public float walkSpeed = 4f;
    public float walkStopRate = 0.05f;

    Rigidbody2D rb;
    TouchingEvents touchingEvents;
    Animator animator;
    AudioManager audioManager;
    DamageController damageController;

    public enum WalkDirection { Right, Left };
    private WalkDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;
    private Transform player;
    private Vector2 playerLastPosition;

    [Header("Under 500HP")]
    public int activeSpawnHP = 500;
    public GameObject skullPrefab;
    public Transform[] leftSpawnPoints;
    public Transform[] rightSpawnPoints;
    public float spawnInterval = 6f;
    private bool isSpawning = false;

    public WalkDirection walkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
                if (value == WalkDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }
                else if (value == WalkDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }

            _walkDirection = value;
        }
    }

    public bool _hasTarget = false;

    public bool HasTarget
    {
        get
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
        damageController = GetComponent<DamageController>();
    }
    private void FixedUpdate()
    {
        //if (touchingEvents.IsWall && touchingEvents.IsTouch)
        //{
        //    FlipDirection();
        //}

        if (CanMove)
        {
            //rb.velocity = new Vector2((player.position.x - transform.position.x), rb.velocity.y) * walkSpeed;
            Vector2 direction = new Vector2(player.position.x - transform.position.x, 0f).normalized;
            rb.velocity = direction * walkSpeed;
        }
        else
        {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && CanMove)
        {
            // Tính toán hướng vector từ Goblin tới người chơi
            Vector2 direction = new Vector2(player.position.x - transform.position.x, 0f).normalized;

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

        if (damageController.Health <= activeSpawnHP)
        {
            StartSpawning();
        }
    }

    public void FireProjectile()
    {
        if (tornadoPrefab && firePoint)
        {
            GameObject tornado = Instantiate(tornadoPrefab, firePoint.position, Quaternion.identity);
            Vector2 fireDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

            tornado.transform.localScale = new Vector3(transform.localScale.x, tornado.transform.localScale.y, tornado.transform.localScale.z);

            tornado.GetComponent<FireballMovement>().Initialize(fireDirection);
        }
    }

    private void FlipDirection()
    {
        if (walkDirection == WalkDirection.Right)
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

    private void OnDestroy()
    {
        if (unknowMagic)
        {
            GameObject magic = Instantiate(unknowMagic, transform.position, Quaternion.identity);

            StopSpawning();

            if (audioManager)
            {
                audioManager.StopMusicSoftly(5f);
            }
        }
    }

    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnBoss());
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
        StopCoroutine(SpawnBoss());
    }

    IEnumerator SpawnBoss()
    {
        while (isSpawning)
        {
            foreach (Transform leftSpawnPoint in leftSpawnPoints)
            {
                SpawnBossAtPoint(leftSpawnPoint);
            }

            foreach (Transform rightSpawnPoint in rightSpawnPoints)
            {
                SpawnBossAtPoint(rightSpawnPoint);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnBossAtPoint(Transform spawnPoint)
    {
        GameObject newSkull = Instantiate(skullPrefab, spawnPoint.position, Quaternion.identity);

        if (spawnPoint.position.x > 50.0f)
        {
            newSkull.transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        // Kích hoạt di chuyển của fireball nếu có
        FireballMovement fireballMovement = newSkull.GetComponent<FireballMovement>();
        if (fireballMovement != null)
        {
            Vector2 direction = (spawnPoint.position.x > 50.0f) ? Vector2.left : Vector2.right;

            fireballMovement.Initialize(direction);
        }
    }

}
