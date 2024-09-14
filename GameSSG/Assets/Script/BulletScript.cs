using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D body;
    public float moveSpeed = 10f;
    public Cat cat;
    private bool right = false;
    private bool up = false;
    private bool down = false;
    [SerializeField] private float lifeSpan = 2f;
    [SerializeField] private float running;
    [SerializeField] private float startfire;
    public ParticleSystem particle;

    [SerializeField] private Vector3 offSetRightUp;
    [SerializeField] private Vector3 offSetLeftUp;
    [SerializeField] private Vector3 offSetRightDown;
    [SerializeField] private Vector3 offSetLeftDown;
    [SerializeField] private Vector3 offSetRight;
    [SerializeField] private Vector3 offSetLeft;
    [SerializeField] private Animator animator;
    private bool isMoving = true;
    void Start()
    {
        cat = GameObject.FindGameObjectWithTag("Player").GetComponent<Cat>();
        if(!cat.isDashing)
        {
            CheckPosition();
        }
        StartCoroutine(BulletAnimation());

        Destroy(gameObject, lifeSpan);


    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
            BulletDirection();
            BulletMove();
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.CompareTag("Ground"))
        {
            StartCoroutine(BulletColliseAnimation()); 
        }
        else if(collision.CompareTag("Enemy"))
        {
            StartCoroutine(BulletColliseEnemy());
        }
        
    }
    private IEnumerator BulletColliseEnemy()
    {
        animator.SetBool("isColliseEnemy", true);
        isMoving = false;
        body.velocity = Vector2.zero;
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("isColliseEnemy", false);
        Destroy(gameObject);
    }
    private IEnumerator BulletAnimation()
    {
        BulletDirection();
        
        BulletMove();
        
        yield return new WaitForSeconds(running);
        GetComponent<BoxCollider2D>().enabled = false;
        body.velocity = Vector2.zero;
        
        isMoving = false;
    }
    private IEnumerator BulletColliseAnimation()
    {
        animator.SetBool("isColliseGround", true);
        body.velocity = Vector2.zero;
        isMoving = false;
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("isColliseGround", false);
        Destroy(gameObject);
    }
    private void CheckPosition()
    {
        if (cat.isFacingRight)
        {
            right = true;
            if(cat.look_updown == 0)
            transform.position += offSetRight;
        }
        else
        {
            right = false;
            if(cat.look_updown == 0) 
            transform.position += offSetLeft;
        }
        if (cat.look_updown > 0)
        {
            if (cat.isFacingRight)
            {
                transform.position += offSetRightUp;
            }
            else
            {
                transform.position += offSetLeftUp;
            }
            up = true;
            down = false;
        }
        else if (cat.look_updown < 0)
        {
            if(!cat.isGrounded)
            {   
                if (cat.isFacingRight)
                {
                    transform.position += offSetRightDown;
                }
                else
                {
                    transform.position += offSetLeftDown;
                }
                down = true;
            }
            else
            {
                if (cat.isFacingRight)
                {
                    transform.position += offSetRight;
                }
                else
                {
                    transform.position += offSetLeft;
                }
                
            }
            up = false;
        }
        else
        {
            up = false;
            down = false;
        }
    }
    private void BulletDirection()
    {
        if (right && !up && !down)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            
        }
        else if (!right && !up && !down)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            
        }
        else if (up)
        {

            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            
        }
        else if (down)
        {

            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            
        }
    }
    private void BulletMove()
    {
        
        if (right && !up && !down)
        {
            
            body.velocity =  new Vector2(moveSpeed,body.velocity.y);
        }
        else if (!right && !up && !down)
        {
            
            body.velocity = new Vector2(-moveSpeed, body.velocity.y);
        }
        else if (up)
        {

            
            body.velocity = new Vector2(body.velocity.x, moveSpeed);
        }
        else if (down)
        {

            
            body.velocity = new Vector2(body.velocity.x,- moveSpeed);
        }
    }
}
