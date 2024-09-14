using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardBulletScript : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject target;
    [SerializeField] private float speed;
     Rigidbody2D rb;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        Vector2 moveDir = (target.transform.position - transform.position).normalized*speed;
        rb.velocity = new Vector2(moveDir.x, moveDir.y);
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ground") || collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        
    }
    void Update()
    {
        
    }
}
