using UnityEngine;
using UnityEngine.SceneManagement; 

[RequireComponent(typeof(BoxCollider2D))]
public class CharacterController2D : MonoBehaviour
{
    [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    float speed = 20;
    /*
        [SerializeField, Tooltip("Acceleration while grounded.")]
        float walkAcceleration = 35;

        [SerializeField, Tooltip("Acceleration while in the air.")]
        float airAcceleration = 15;

        [SerializeField, Tooltip("Deceleration applied when character is grounded and not attempting to move.")]
        float groundDeceleration = 90;


        [SerializeField, Tooltip("Max height the character will jump regardless of gravity")]
        float jumpHeight = 3;
    */
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

    //////////////////////////////////////////////////

    private BoxCollider2D boxCollider;

    private Vector2 velocity;

    // removed Ground -zoe

    public float fireRate = 0.2f;

    private float nextFire = 0.0f;

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

        // added Rigidbody2D as rb for more ctrl? -zoe
        rb = GetComponent<Rigidbody2D>();

    }

    // chnaged from Update to FixedUpdate -zoe 
    // Optimized -zoe 
    private void FixedUpdate()
    {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        coyoteTimeCounter = isGrounded ? coyoteTime : coyoteTimeCounter -= Time.deltaTime;



        //use GetAxisRaw for more Snappy movement if desired -zoe
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);


        // make charecter Sprite face the direction its moving... 
        // if we have seprate sprites for rigth and left howver we can remove this -zoe
        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }
    }

    void Update()
    {   
        if (damageTime > 0f){
            damageTime -= Time.deltaTime;
        }



        // new jump function -zoe /////

       extraJumps = coyoteTimeCounter > 0f ? extraJumpValue : extraJumps;


        if (Input.GetButtonDown("Jump") && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpforce;
            extraJumps--;
        }
        else if (jumpBufferCounter > 0f && extraJumps == 0 && isGrounded == true)
        {
            rb.velocity = Vector2.up * jumpforce;
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



        /////////////////////////

    
        if (Input.GetMouseButton(1) && (Time.time > nextFire))
        {
            GameObject bulletGameObject = Instantiate(playerBulletPrefab, transform.position, transform.rotation);
            Rigidbody2D bullet = bulletGameObject.GetComponent<Rigidbody2D>();
            float xspeed;
            // if facing right shoot right if facing left shoot left -zoe
            xspeed = !facingRight ? -20.0f : 20.0f;
            bullet.velocity = new Vector2(xspeed, 0.0f);
            nextFire = Time.time + fireRate;
        }

        //kept second mouse button dont know if you guys still need it -zoe
        if (Input.GetMouseButton(0) && (Time.time > nextFire))
        {
            GameObject bulletGameObject = Instantiate(playerBulletPrefab, transform.position, transform.rotation);
            Rigidbody2D bullet = bulletGameObject.GetComponent<Rigidbody2D>();
            float xspeed;
            // if facing right shoot right if facing left shoot left -zoe
            xspeed = !facingRight ? -20.0f : 20.0f;
            bullet.velocity = new Vector2(xspeed, 0.0f);
            nextFire = Time.time + fireRate;
        }
    }


    // Flips charecter Sprite to opposite of whatever there facing -zoe
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    void OnTriggerStay2D(Collider2D other){
        if (damageTime<=0){
            damageTime = damageTimeMax;
            Debug.Log("fuck");
            hearts healthController =  GetComponent<hearts>();
            if(healthController.health <= 1 ){
                SceneManager.LoadScene(2);
            }
            else {
                healthController.health--;
            }
        }
    }


}
