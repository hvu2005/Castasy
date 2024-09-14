using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    public float fadeSpeed = 1f;
    public SpriteRenderer spriteRenderer;
    private Color color;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;
    }

    void Update()
    {
        
        color.a -= fadeSpeed * Time.deltaTime;
        spriteRenderer.color = color;

        
        if (color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
