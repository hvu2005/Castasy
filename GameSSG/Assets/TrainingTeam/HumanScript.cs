using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HumanScript : MonoBehaviour
{
    public float moveSpeed;
    [SerializeField] private float move;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float jumpForce;
    [SerializeField] private float checkJump;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isFacingRight;
    [SerializeField] private Vector2 rayOffset;
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private float dashingPowder;
    [SerializeField] private bool isDashing = false;
    [SerializeField] private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 4;
        isFacingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDashing)
        {
            return;
        }
        movingBehave();
        Jump();
        Flip();
        checkEnemy();
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dash());
            StartCoroutine(Duck());
        }
        if(move > 0 || move < 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        if(!isGrounded)
        {
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }
    }
    private void checkEnemy()
    {
        Vector2 position = (Vector2)transform.position + rayOffset;
        Vector2 direction = isFacingRight ?  Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(position, direction, 5f, whatIsEnemy);
        Debug.DrawRay(position, direction*5f, Color.green);
        if(hit.collider != null)
        {
            Debug.Log("sss");
        }
    }
    private void movingBehave()
    {
        move = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2 (moveSpeed*move, rb.velocity.y);
        
    }
    private void Flip()
    {
        if(move < 0f && isFacingRight || move > 0f && !isFacingRight)
        {
            isFacingRight = !isFacingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
        
    }
    private IEnumerator Dash() 
    {
        animator.SetBool("isDashing", true);
        isDashing = true;
        float originGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPowder, rb.velocity.y);
        yield return new WaitForSeconds(0.5f);        
        isDashing = false;
        rb.gravityScale = originGravity;
        animator.SetBool("isDashing", false);

    }
    private IEnumerator Duck()
    {
        float timeElipesed = 0f;
        while(timeElipesed < 0.2f)
        {
            Debug.Log("in ra");
            timeElipesed += Time.deltaTime;
        }
        yield return null;
    }
    private void Jump()
    {
        checkJump = Input.GetAxis("Vertical");

        if (checkJump > 0 && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Ground") )
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

}
