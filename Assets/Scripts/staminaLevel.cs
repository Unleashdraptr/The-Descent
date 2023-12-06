using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class staminaLevel : MonoBehaviour
{
    private float originalWidth;
    private SpriteRenderer skin;
    private Movement stamina;
    private Vector3 originalPosition;
    // Start is called before the first frame update
    void Start()
    {
        skin = GetComponent<SpriteRenderer>();
        originalWidth = skin.size.x;
        stamina = GetComponentInParent<Movement>();
        originalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float newWidth = originalWidth*(stamina.stamina/100);
        skin.size = new Vector2(newWidth,skin.size.y);
        transform.localPosition = new Vector2(originalPosition.x-(((100-stamina.stamina)/200)*originalWidth),transform.localPosition.y);
    }
}
