// zoe - 2023

/*
This is a new and Improved Charecter Controller for snappy tight movement
instead <3


The MIT License (MIT)
Copyright © 2023 <copyright holders>
Permission is hereby granted, free of charge, to any person obtaining a copy of this software
and associated documentation files (the “Software”), to deal in the Software without
restriction, including without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies
or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace zoe
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class NewCharecterController2D : MonoBehaviour
    {
        [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
        public float speed = 9;

        [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
        public float Deceleration = 13;

        [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
        public float Acceleration = 13;

        [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
        public float velPower = 0.96f;

        [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
        public float frictionAmount = 0.96f;

        [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
        public float jumpCutMultiplier = 0.4f;

        [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
        public float fallGravityMultiplier = 2;

	    private float gravityScale;

        // zoe - ///////////////////////////////////////////

        [SerializeField, Tooltip("Max height the character will jump regardless of gravity")]
        public float jumpforce = 25.3f;

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

        // added coyote Jump and Jump buffer -zoe

        /*
    
        explination: Coyote Jump and Jump Buffer allows for lee way when jumping too late or too  early
        this intern makes the player experince feel alot more smooth.
    
        */
        private float coyoteTime = 0.15f;
        private float coyoteTimeCounter;

        private float jumpBuffer = 0.15f;
        private float jumpBufferCounter;

        private float jumpTimer = 0.15f;
        private float jumpTimeCounter;
 

        private bool isJumping;

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            extraJumps = extraJumpValue;
            

            // added Rigidbody2D as rb for more ctrl? -zoe
            rb = GetComponent<Rigidbody2D>();
            gravityScale = rb.gravityScale;
        }

        // chnaged from Update to FixedUpdate -zoe
        // Optimized -zoe
        private void FixedUpdate()
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
            coyoteTimeCounter = isGrounded ? coyoteTime : coyoteTimeCounter -= Time.deltaTime;

            // move
            moveInput = Input.GetAxis("Horizontal");
            float targetSpeed = moveInput * speed;
            float diffrence = targetSpeed - rb.velocity.x;
            float accel = (Mathf.Abs(targetSpeed) > 0.01f) ? Acceleration : Deceleration;
            float movement = Mathf.Pow(Mathf.Abs(diffrence) * accel, velPower) * Mathf.Sign(diffrence);
            rb.AddForce(movement * Vector2.right);

            //friction
            if (isGrounded && Mathf.Abs(moveInput) < 0.01f)
            {
                float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));
                amount *= Mathf.Sign(rb.velocity.x);
                rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
            }

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
            //Debug.Log(Time.deltaTime);
            // new jump function -zoe /////

            extraJumps = coyoteTimeCounter > 0f ? extraJumpValue : extraJumps;
            /*
            if (Input.GetButtonDown("Jump") && extraJumps > 0)
            {
                rb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
                extraJumps--;
                OnJumpUp();
                isJumping = true;
            }
            else if (jumpBufferCounter > 0f && extraJumps == 0 && isGrounded == true)
            {
                Debug.Log("fuck");
                rb.AddForce(Vector2.up * jumpforce , ForceMode2D.Impulse);
                OnJumpUp();
                isJumping = true;
            }*/

             if (Input.GetButtonDown("Jump") && isGrounded == true)
            {
                rb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
                //extraJumps--;
                OnJumpUp();
                isJumping = true;
            }

            if (Input.GetButtonUp("Jump"))
            {
                coyoteTimeCounter = 0f;
                //jumpBufferCounter = 0f;
            }

            if (Input.GetButtonDown("Jump"))
            {
               jumpBufferCounter = jumpBuffer;
               


                //jumpTimeCounter = jumpBuffer;
            }
            else
            {
               jumpBufferCounter -= Time.deltaTime;
                //jumpTimeCounter -= Time.deltaTime;
                
            }

            if (rb.velocity.y < 0)
            {
                isJumping = false;
                rb.gravityScale = gravityScale * fallGravityMultiplier;
            }else {
                rb.gravityScale = gravityScale;
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
        
        public void OnJumpUp()
        {
            if (rb.velocity.y > 0 && isJumping && !Input.anyKey  )
            {        
                rb.AddForce(Vector2.down * rb.velocity.y * jumpCutMultiplier, ForceMode2D.Impulse);
            }
        }
    }
}