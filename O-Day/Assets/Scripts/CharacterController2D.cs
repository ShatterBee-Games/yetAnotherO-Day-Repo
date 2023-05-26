using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CharacterController2D : MonoBehaviour
{
    [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    float speed = 15;

    [SerializeField, Tooltip("Acceleration while grounded.")]
    float walkAcceleration = 35;

    [SerializeField, Tooltip("Acceleration while in the air.")]
    float airAcceleration = 15;

    [SerializeField, Tooltip("Deceleration applied when character is grounded and not attempting to move.")]
    float groundDeceleration = 90;

    [SerializeField, Tooltip("Max height the character will jump regardless of gravity")]
    float jumpHeight = 3;

    [SerializeField, Tooltip("Prefab for bullet")]
    GameObject playerBulletPrefab;

    private BoxCollider2D boxCollider;

    private Vector2 velocity;

    private bool grounded;

    public float fireRate = 0.2f;

    private float nextFire = 0.0f;


    private void Awake()
    {      
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {   
        if (grounded)
        {
	        velocity.y = 0;

            if (Input.GetButtonDown("Jump"))
            {
                // Calculate the velocity required to achieve the target jump height.
                velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y));
            }
        }
         
        velocity.y += Physics2D.gravity.y * Time.deltaTime;

        float moveInput = Input.GetAxisRaw("Horizontal");
        float acceleration = grounded ? walkAcceleration : airAcceleration;
        float deceleration = grounded ? groundDeceleration : 0;




        if (Input.GetMouseButton(1) && (Time.time > nextFire) ){
            GameObject bulletGameObject = Instantiate(playerBulletPrefab, transform.position, transform.rotation);
            Rigidbody2D bullet = bulletGameObject.GetComponent<Rigidbody2D>();
            float xspeed = 20.0f;
            bullet.velocity = new Vector2(xspeed, 0.0f);
            nextFire = Time.time + fireRate;
        }

        if (Input.GetMouseButton(0)  && (Time.time > nextFire)){
            GameObject bulletGameObject = Instantiate(playerBulletPrefab, transform.position, transform.rotation);
            Rigidbody2D bullet = bulletGameObject.GetComponent<Rigidbody2D>();
            float xspeed = -20.0f;
            bullet.velocity = new Vector2(xspeed, 0.0f);
            nextFire = Time.time + fireRate;
        }


        // Update the velocity assignment statements to use our selected
        // acceleration and deceleration values.
        if (moveInput != 0)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInput, acceleration * Time.deltaTime);
        }
        else
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
        }

            transform.Translate(velocity * Time.deltaTime);

            Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);
            
            grounded = false;


            foreach (Collider2D hit in hits){
                if (hit == boxCollider)
                continue;

                if (hit.gameObject.layer == 3)
                continue;

                ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

                if (colliderDistance.isOverlapped){
                    transform.Translate(colliderDistance.pointA - colliderDistance.pointB);
                }
                
                if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && velocity.y < 0){
                    grounded = true;
                }
            }
        }
}
