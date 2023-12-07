using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    private Health health;
    private bool dashing;
    private bool dashReset;
    private float dashTimer;
    private float dashSpeed;
    private bool rightLeft; // right is false, tracks last horizontal direction

    private bool dashHorizontal; // tracks wether or not the player should dash horizontally
    private bool dashRightLeft; // right is false, tracks horizontal direction of next dash

    private bool dashVertical; // tracks wether or not the player should dash vertically
    private bool dashDownUp; // down is false, tracks vertical direction of next dash

    private float slide;
    private bool rightLock;
    private bool leftLock;
    private bool wallJump;
    private bool jumpingIntoAWall; // ugh
    private bool sliding; // lmao i solve all my problems by just adding extra variables
    //private bool haveWallJumped; // use ctr+F to find then add back in all instances. this will make it so that you can only wall jump once, but is unstable
    
    [SerializeField] private bool doubleJump;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask deadlyGround;
    [SerializeField] public float stamina;

    private enum MovementState { idle, running, jumping, falling }

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        stamina = 100f;
        dashTimer = 1;
    }

    // Update is called once per frame
    private void Update()
    {
        bool dash = Input.GetButtonDown("Dash");
        bool up = Input.GetButton("Up");
        bool down = Input.GetButton("Down");
        bool left = Input.GetButton("Left");
        bool right = Input.GetButton("Right");

        float dirX = Input.GetAxis("Horizontal");
        float extraSpeed = Input.GetAxisRaw("Sprint");
        
        if (!health.isDead)
        {
            if (right)
            {
                dashRightLeft = false;
            }

            if (dirX > 0)
            {
                sprite.flipX = false;
            }

            if (left)
            {
                dashRightLeft = true;
            }

            if (dirX < 0)
            {
                sprite.flipX = true;
            }


            //dash input control
            if (dash && !dashReset)
            {
                dashReset = true;
                dashing = true;
            }

            if (!dash)
            {
                dashReset = false;
            }

            //initiate dash and dash direction
            if (stamina >= 50f && dashTimer == 1 && dash)
            {
                stamina -= 50;
                dashTimer = 20;
                dashSpeed = 20;
                dashing = false;

                // checks direction when the dashing is initiated so it can't change direction during
                if (up || down)
                {
                    dashVertical = true;

                    if (!left && !right)
                    {
                        dashHorizontal = false;
                    }

                    else
                    {
                        dashHorizontal = true;
                    }
                }

                else
                {
                    dashVertical = false;
                    dashHorizontal = true;
                }

                if (down)
                {
                    dashDownUp = false;
                }

                if (up)
                {
                    dashDownUp = true;
                }

            }

            //change player position based on dash
            if (dashTimer > 1)
            {
                if (dashHorizontal)
                {
                    if (dashRightLeft)
                    {
                        rb.velocity = new Vector2(-dashSpeed, 0);
                    }

                    if (!dashRightLeft)
                    {
                        rb.velocity = new Vector2(dashSpeed, 0);
                    }
                }

                if (dashVertical)
                {
                    if (dashDownUp)
                    {
                        rb.velocity = new Vector2(rb.velocity.x, dashSpeed);
                    }

                    if (!dashDownUp)
                    {
                        rb.velocity = new Vector2(0, -dashSpeed);
                    }

                    if (!dashHorizontal)
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }

                    if (dashHorizontal)
                    {
                        rb.velocity = new Vector2(rb.velocity.x / 1.4f, rb.velocity.y / 1.4f);
                    }
                }

                dashTimer -= 1f;
                dashSpeed -= 0.74f; //initial dashSpeed / (initial dashTimer + 7)
            }



            if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + 10f);
            }


            if (stamina < 100f && IsGrounded())
            {
                stamina += .6f;
            }


            // change gravity (after letting go) and sensitivity (after pressing down) in input manager to change floatiness

            if (dashTimer == 1 && (dirX < 0 && !WallLeft() || dirX > 0 && !WallRight()) && !wallJump)
            {
                rb.velocity = new Vector2(dirX * 7f, rb.velocity.y);
            }


            if (wallJump)
            {
                if (rb.velocity.x > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x - 0.1f, rb.velocity.y);
                }
                if (rb.velocity.x < 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x + 0.1f, rb.velocity.y);
                }

                if (dirX * 7 > rb.velocity.x && !rightLock || dirX * 7 < rb.velocity.x && !leftLock)
                {
                    rb.velocity = new Vector2(dirX * 7f, rb.velocity.y);
                }
            }

            if (IsGrounded())
            {
                wallJump = false;
                //haveWallJumped = false;
                sliding = false;
                slide = 0f;

                if (dirX != 0)
                {
                    anim.SetBool("Running", true);
                }
                else
                {
                    anim.SetBool("Running", false);
                }
            }

            if (!IsGrounded())
            {
                anim.SetBool("Running", false);
            }



            if (WallLeft() && left && Input.GetButtonDown("Jump") && !sliding)
            {
                jumpingIntoAWall = true;
            }

            if (Input.GetButtonDown("Left"))
            {
                jumpingIntoAWall = false;
            }

            if (WallLeft() && left && !IsGrounded() && !jumpingIntoAWall)
            {
                rb.velocity = new Vector2(0, slide);
                slide -= 0.035f;
                sliding = true;
                wallJump = false;

                if (Input.GetButtonDown("Jump") && !wallJump) // add "&& !haveWallJumed"
                {
                    rb.velocity = new Vector2(7f, 10f);
                    leftLock = true;
                    wallJump = true;
                    //haveWallJumped = true;
                    sliding = false;
                }
            }

            if (WallRight() && right && Input.GetButtonDown("Jump") && !sliding)
            {
                jumpingIntoAWall = true;
            }

            if (Input.GetButtonDown("Right"))
            {
                jumpingIntoAWall = false;
            }

            if (WallRight() && right && !IsGrounded() && !jumpingIntoAWall)
            {
                rb.velocity = new Vector2(0, slide);
                slide -= 0.035f;
                sliding = true;
                wallJump = false;

                if (Input.GetButtonDown("Jump") && !wallJump) // add "&& !haveWallJumed"
                {
                    rb.velocity = new Vector2(-7f, 10f);
                    rightLock = true;
                    wallJump = true;
                    //haveWallJumped = true;
                    sliding = false;
                }
            }

            if (rightLock && Input.GetButtonDown("Right"))
            {
                rightLock = false;
                wallJump = false;
            }

            if (leftLock && Input.GetButtonDown("Left"))
            {
                leftLock = false;
                wallJump = false;
            }
        }

        else
        {
            anim.SetBool("Dead", true);
            sprite.color = new Color(1, 1, 1, 0.5f);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.min + coll.bounds.size / 2, coll.bounds.size * 0.9f, 0f, Vector2.down, .1f, jumpableGround);
    }

    private bool WallLeft()
    {
        return Physics2D.BoxCast(coll.bounds.min + coll.bounds.size / 2, coll.bounds.size * 0.9f, 0f, Vector2.left, .1f, jumpableGround);
    }

    private bool WallRight()
    {
        return Physics2D.BoxCast(coll.bounds.min + coll.bounds.size / 2, coll.bounds.size * 0.9f, 0f, Vector2.right, .1f, jumpableGround);
    }
    public bool isOnTrap()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, deadlyGround);
    }

}
