using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider2D))]
public class CharacterController2D : MonoBehaviour
{
    Controls _controls;

    public Animator m_Animator;

    CameraShaker cameraShaker;
    [SerializeField] GameObject Shake;

    [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    float speed = 20;

    [SerializeField, Tooltip("Prefab for bullet")]
    GameObject playerBulletPrefab;


    // zoe - ///////////////////////////////////////////

    [SerializeField, Tooltip("Max height the character will jump regardless of gravity")]
    public float jumpforce;

    private float moveInput;
    private Rigidbody2D rb;
    private int extraJumps;

    [SerializeField, Tooltip("Any extra amount of jumps?")]
    public int extraJumpValue;

    private bool isGrounded;

    [SerializeField, Tooltip("Object To check if Player is On ground")]
    public Transform groundCheck;

    [SerializeField, Tooltip("How far does the Object check (Recommend 0.5) ")]
    public float checkRadius;

    [SerializeField, Tooltip("Which Tags are considered Ground")]
    public LayerMask whatIsGround;

    private bool facingRight = true;
    private bool running = false;

    //////////////////////////////////////////////////

    private BoxCollider2D boxCollider;

    private Vector2 velocity;

    // removed Ground -zoe

    public float fireRate = 0.2f;

    private float nextFire = 0.0f;

    int bulletCount = 0;

    [SerializeField, Tooltip("maximum bullets per case")]
    int bulletCountMax = 5;

    float reloadTimer = 0f;

    [SerializeField, Tooltip("reload time")]
    float reloadTimeerMax = 1.5f;

    // added coyote Jump and Jump buffer -zoe

    /*

    explination: Coyote Jump and Jump Buffer allows for lee way when jumping too late or too  early
    this intern makes the player experince feel alot more smooth.

    */
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private float jumpBuffer = 0.2f;
    private float jumpBufferCounter;

    float damageTime;

    [SerializeField, Tooltip("Immortality after damage taken time")]
    float damageTimeMax = 1.0f;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        extraJumps = extraJumpValue;

        _controls = new Controls();
        m_Animator.SetBool("Facing_Right", facingRight);
        m_Animator.SetBool("isRunning", running);

        _controls.Player.Shoot.performed += ctx => Onfire();

        // added Rigidbody2D as rb for more ctrl? -zoe
        rb = GetComponent<Rigidbody2D>();
        //this.gameObject.transform.GetChild(0)
        m_Animator = this.gameObject.transform.GetChild(0).GetComponent<Animator>();

        cameraShaker = Shake.GetComponent<CameraShaker>();
      


        bulletCount = bulletCountMax;

        bulletCounter bulletUI = GetComponent<bulletCounter>();
        bulletUI.bullets = bulletCount;
        // bulletUI.reloadBar.enabled = false;
    }

    // chnaged from Update to FixedUpdate -zoe
    // Optimized -zoe
    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        coyoteTimeCounter = isGrounded ? coyoteTime : coyoteTimeCounter -= Time.deltaTime;

        //use GetAxisRaw for more Snappy movement if desired -zoe
        moveInput = Input.GetAxis("Horizontal");
        m_Animator.SetFloat("moveinput", moveInput);

        m_Animator.SetBool("isRunning", running);

        running = moveInput != 0;

        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // make charecter Sprite face the direction its moving...
        // if we have seprate sprites for rigth and left howver we can remove this -zoe
        if ((facingRight && moveInput < 0) || (!facingRight && moveInput > 0))
        {
            Flip();
        }
 
    }

    void Update()
    {
        if (damageTime > 0f)
        {
            damageTime -= Time.deltaTime;
        }

        if (reloadTimer > 0f)
        {
            reloadTimer -= Time.deltaTime;
            bulletCounter bulletUI = GetComponent<bulletCounter>();
            bulletUI.progress = reloadTimer / reloadTimeerMax;
            if (reloadTimer <= 0f)
            {
                bulletCount = bulletCountMax;
                bulletUI.bullets = bulletCount;
                // bulletUI.reloadBar.enabled = false;
            }
        }

        // new jump function -zoe /////

        extraJumps = coyoteTimeCounter > 0f ? extraJumpValue : extraJumps;

        if (Input.GetButtonDown("Jump") && extraJumps > 0)
        {
            Jump();
            extraJumps--;
        }
        else if (jumpBufferCounter > 0f && extraJumps == 0 && isGrounded == true)
        {
            Jump();
        }

        if (Input.GetButtonUp("Jump"))
        {
            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBuffer;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    // Flips charecter Sprite to opposite of whatever there facing -zoe
    void Flip()
    {
        facingRight = !facingRight;
        m_Animator.SetBool("Facing_Right", facingRight);
        /*
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;*/
    }

    void ProcessDamage()
    {
        if (damageTime <= 0)
        {
            damageTime = damageTimeMax;
            Debug.Log("fuck");
            cameraShaker.BasicShake(0.5f, 0.1f);
            hearts healthController = GetComponent<hearts>();
            if (healthController.health <= 1)
            {
                SceneManager.LoadScene(2);
            }
            else
            {
                healthController.health--;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ProcessDamage();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        ProcessDamage();
    }

    void Onfire()
    {
        if ((Time.time > nextFire) && bulletCount > 0)
        {
            GameObject bulletGameObject = Instantiate(
                playerBulletPrefab,
                transform.position,
                transform.rotation
            );
            Rigidbody2D bullet = bulletGameObject.GetComponent<Rigidbody2D>();
            float xspeed;
            // if facing right shoot right if facing left shoot left -zoe
            xspeed = !facingRight ? -20.0f : 20.0f;
            bullet.velocity = new Vector2(xspeed, 0.0f);
            nextFire = Time.time + fireRate;
            bulletCount--;
            bulletCounter bulletUI = GetComponent<bulletCounter>();
            bulletUI.bullets = bulletCount;
            if (bulletCount <= 0)
            {
                reloadTimer = reloadTimeerMax;
                // bulletUI.reloadBar.enabled = true;
            }
        }
    }

    void Jump()
    {
        rb.velocity = Vector2.up * jumpforce;
    }

    void OnEnable()
    {
        _controls.Player.Enable();
    }

    void OnDisable()
    {
        _controls.Player.Disable();
    }
}
