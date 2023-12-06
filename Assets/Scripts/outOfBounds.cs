using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outOfBounds : MonoBehaviour
{
    private Rigidbody2D rb;
    private spawnPoint sp;
    private Health hp;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<spawnPoint>();
        hp = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -30f)
        {
            hp.health = 0;
        }
    }
}
