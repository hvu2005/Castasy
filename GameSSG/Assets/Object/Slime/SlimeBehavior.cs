using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;


public class SlimeBehavior : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private Rigidbody2D rb;
    public GameObject pointA;
    public GameObject pointB;
    [SerializeField] private Transform currentTransform;
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    [SerializeField] private DmgFlash _dmgFlash;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float detectionRange;
    private Cat cat;
    private bool isMoving = true;
    private Transform playerTransform;
    private Vector3 originalPosition;
    private bool isFollowingPlayer = false;
    [SerializeField] private Vector2 rayOffSet;
    private bool isFacingRight = false;
    [SerializeField] BoxCollider2D triggerBox;
    [SerializeField] SpriteRenderer noitice;
    private bool isScaling = false;
    void Start()
    {
        currentTransform = pointA.transform;
        animator.SetBool("isRunning", true);
        originalPosition = transform.position;
        cat = GameObject.FindGameObjectWithTag("Player").GetComponent<Cat>();
        noitice.transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(hp == 0)
        {
            StartCoroutine(Dying());
        }
        Flip();
    }
    private void Flip()
    {
        if (isFacingRight && rb.velocity.x < 0f || rb.velocity.x > 0 && !isFacingRight)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }
    void FixedUpdate()
    {
        if(isMoving)
        {
            DetectPlayer();
            if (isFollowingPlayer)
            {
                FollowPlayer();
            }
            else
            {
                ReturnToPatrol();
            }
        }
            
    }
    private void DetectPlayer()
    {
        Vector2 Ray = (Vector2)transform.position + rayOffSet;
        Vector2 rayDirection = isFacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(Ray, rayDirection, detectionRange, playerLayer);
        
        Debug.DrawRay(Ray, rayDirection* detectionRange, Color.yellow);
        if (hit.collider != null)
        {
            noitice.enabled = true;
            if(isScaling)
            {
                StartCoroutine(ScaleNoitice());
            }
            
            playerTransform = hit.transform;
            isFollowingPlayer = true;
        }
        else
        {
            isScaling = true;
            noitice.transform.localScale = Vector3.zero;
            noitice.enabled=false;
            isFollowingPlayer= false;
        }
    }
    private IEnumerator ScaleNoitice()
    {
        isScaling = false;
        float timeElapsed = 0f;

        while (timeElapsed < 0.1f)
        {
            noitice.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timeElapsed / 0.1f);
            timeElapsed += Time.deltaTime;
            yield return null;
            
        }
        
        noitice.transform.localScale = Vector3.one;

    }
    private void FollowPlayer()
    {
        if(playerTransform != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            if(Vector2.Distance(transform.position, playerTransform.position) > 0f)
            {
                rb.velocity = new Vector2(Mathf.Sign(direction.x) * speed , 0f);
            }
            
        }
    }
    private void ReturnToPatrol()
    {
       
        
            if (isFollowingPlayer == false)
            {

                CheckDistance();
            }
            Patrol();
        
        
        
        
        
    }
    private void  Patrol()
    {
        
        if (currentTransform == pointB.transform)
        {
            rb.velocity = new Vector2(speed, 0f);
        }
        else if(currentTransform == pointA.transform) 
        {
            rb.velocity = new Vector2(-speed, 0f);
        }
        

    }
    private void CheckDistance()
    {
        if (Vector2.Distance(transform.position, currentTransform.position) < 0.5f )
        {
            if (currentTransform == pointB.transform)
            {
                
                currentTransform = pointA.transform;
            }
            else if (currentTransform == pointA.transform)
            {

                currentTransform = pointB.transform;
            }
        }
        if(!isFollowingPlayer)
        {
            if (Vector2.Distance(transform.position, pointA.transform.position) > Vector2.Distance(pointB.transform.position, pointA.transform.position) ||
            Vector2.Distance(transform.position, pointB.transform.position) > Vector2.Distance(pointB.transform.position, pointA.transform.position))
            {

                if (Vector2.Distance(transform.position, pointA.transform.position) < Vector2.Distance(transform.position, pointB.transform.position))
                {
                    currentTransform = pointB.transform;
                }
                else
                {
                    currentTransform = pointA.transform;
                }


            }
        }
    }
    private IEnumerator WaitAnimation() 
    {
        isMoving = false;
        animator.SetBool("isRunning", false);
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1f);
        isMoving = true;
        animator.SetBool("isRunning", true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
            if (collision.CompareTag("Bullet"))
            {
                if(cat.isFacingRight && cat.look_updown == 0)
                {
                
                    transform.position += new Vector3(0.2f, 0f, 0f);
                }
                else if (!cat.isFacingRight && cat.look_updown == 0)
                {
                
                    transform.position += new Vector3(-0.2f, 0f, 0f);
                }

                hp--;
                _dmgFlash.CallDmgFlash();
            }
        
    }
    
    private IEnumerator Dying()
    {
        noitice.enabled = false;
        animator.SetBool("isDead", true);
        rb.velocity = Vector2.zero;
        isMoving = false;
        GetComponent<BoxCollider2D>().enabled = false;
        triggerBox.enabled = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);

    }

}
