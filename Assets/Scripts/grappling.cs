using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grappling : MonoBehaviour
{
    private BoxCollider2D coll;
    private Rigidbody2D rb;

    [SerializeField] public float lineLength;
    [SerializeField] public bool isGrounded;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private GameObject player;
    private objectSpawner playerscript;
    private Rigidbody2D playerRB;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        isGrounded = IsGrounded();
        player = GameObject.Find("Player");
        playerscript = player.GetComponent<objectSpawner>();
        playerRB = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float extend = Input.GetAxisRaw("Launch Grapple");
        isGrounded = IsGrounded();
        if (isGrounded == true)
        {
            rb.simulated = false;
            if (extend > 0)
            {
                // Increase the line length
                lineLength += 0.1f;
            }
            else if (extend < 0)
            {
                // Decrease the line length. Must be greater than zero
                //                           to avoid divide by zero error.
                lineLength -= 0.1f;
                if (lineLength < 1f)
                {
                    lineLength = 1f;
                }
            }

            // Force the player to move towards the grappling hook if it is further away than lineLength
            if (playerscript.distance() > lineLength)
            {
                playerRB.AddForce(calculateForce(), ForceMode2D.Impulse);
            }

        }
        else
        {
            rb.simulated = true;
            lineLength = playerscript.distance();
        }
    }
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
    private Vector3 calculateForce()
    {
        Vector3 forceVector = transform.position-player.transform.position;
        float forceSize = playerscript.vector3ToScalar(forceVector);
        forceVector = forceVector/forceSize;
        forceVector = forceVector*(playerscript.distance()-lineLength);
        return forceVector;
    }
}
