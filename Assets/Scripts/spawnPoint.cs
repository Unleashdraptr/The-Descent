using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnPoint : MonoBehaviour
{
    [SerializeField] public Vector2 checkpoint;
    private Vector2 spawnpoint;
    // Start is called before the first frame update
    void Start()
    {
        checkpoint = transform.position;
        spawnpoint = checkpoint;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void resetCheckPoint()
    {
        checkpoint = spawnpoint;
    }
}
