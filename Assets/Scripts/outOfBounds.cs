using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outOfBounds : MonoBehaviour
{
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -30f)
        {
            transform.position = new Vector3(-15.86f,-17f,0);
            rb.velocity = new Vector2(0,0);
        }
    }
}
