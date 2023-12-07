using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthLevel : MonoBehaviour
{
    [SerializeField]private float originalWidth;
    private SpriteRenderer skin;
    [SerializeField] private Health hp;
    private Vector3 originalPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        skin = GetComponent<SpriteRenderer>();
        originalWidth = skin.size.x;
        hp = GetComponentInParent<Health>();
        originalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float newWidth = originalWidth*(hp.health/100);
        skin.size = new Vector2(newWidth,skin.size.y);
        transform.localPosition = new Vector2(originalPosition.x-(((100-hp.health)/200)*originalWidth),transform.localPosition.y);
    }
}
