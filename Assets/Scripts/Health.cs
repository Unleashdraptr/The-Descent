using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public float health;
    [SerializeField] public bool isDead;
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
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (mvmt.isOnTrap())
        {
            health -= 25;
            if (health <= 0)
            {
                sp.resetCheckPoint();
                die();
                health = 0;
            }
            else
            {respawn();}
        }
        if (isDead && rb.velocity.y <= -5f)
        {
            rb.simulated = false;
        }
    }
    public void respawn()
    {
        transform.position = new Vector3(sp.checkpoint.x,sp.checkpoint.y,0);
        rb.velocity = new Vector2(0,0);
    }
    public void die()
    {
        isDead = true;
        rb.velocity = new Vector2(0f,10f);
    }
}
