using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    
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
        doubleJump = true;
        stamina = 100;
    }

    // Update is called once per frame
    private void Update()
    {
        float dirX = Input.GetAxis("Horizontal");
        float extraSpeed = Input.GetAxisRaw("Sprint");
        
        if (stamina > 0)
        {
            rb.velocity = new Vector2(dirX * (7f + (extraSpeed*21*(stamina/100))), rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(dirX * (3.5f), rb.velocity.y);
        }

        if (extraSpeed > 0 && Mathf.Abs(dirX) > 0)
        {
            if (stamina > 0)
            {
                stamina -= 1 * Time.deltaTime*300;
            }
        }
        else if (stamina < 100)
        {
            stamina += 1 * Time.deltaTime*300;
        }
        if (stamina > 100)
        {
            stamina = 100;
        }
        else if (stamina < 0)
        {
            stamina = 0;
        }

        if (dirX > 0)
        {
            sprite.flipX = false;
        }
        else if (dirX < 0)
        {
            sprite.flipX = true;
        }

        if (Input.GetButtonDown("Jump") && (IsGrounded() || doubleJump))
        {
            rb.velocity = new Vector2(rb.velocity.x, 10f);
            if (IsGrounded() == false)
            {
                doubleJump = false;
            }
        }
        if (IsGrounded())
        {
            doubleJump = true;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.min, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
    public bool isOnTrap()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, deadlyGround);
    }
}
