using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public float health;
    private Rigidbody2D rb;
    private spawnPoint sp;
    private Movement mvmt;
    
    // Start is called before the first frame update
    void Start()
    {
        health = 100;   
        sp = GetComponent<spawnPoint>();
        rb = GetComponent<Rigidbody2D>();
        mvmt = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            sp.resetCheckPoint();
            die();
            health = 100;
        }
        if (mvmt.isOnTrap())
        {
            health -= 25;
            die();
        }
    }
    public void die()
    {
        transform.position = new Vector3(sp.checkpoint.x,sp.checkpoint.y,0);
        rb.velocity = new Vector2(0,0);
    }
}
