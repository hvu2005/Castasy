using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Cat : MonoBehaviour
{
    //
    
    //
    public CameraShake cameraShake;

    //moving
    
    private bool canMove = true;
    public float jumpForce;
    public float moveSpeed;
    public Rigidbody2D body;
    public float move;
    public bool isGrounded = true;
    public bool isFacingRight = true;
    public float look_updown;
    //Dash
    public bool canDash = true;
    public bool isDashing;
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public bool dashing = true;
    public GameObject afterimagePrefab;  
    public float afterimageSpawnInterval = 0.05f;
    //Cant moving
    public Fire fire;
    //checkGround
    public float groundCheckDistance = 0.1f;
    public LayerMask whatIsGround;
    [SerializeField] private Vector2 RayGroundOffset;
    //checkWall
    public float wallCheckDistance = 0.5f;
    //jump
    private float jumpTimeCounter;
    [SerializeField] private float jumpTime = 0.5f;
    [SerializeField] public bool CheckFalling;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool CheckJumping;
    //collision
    [SerializeField] private bool isTouchingWall;

    //
    [SerializeField] private GameObject shotgun;
    //takeDmg
    public bool isTakeDmg = false;
    //invis
    private bool isInvis = false;
    [SerializeField] private SpriteRenderer catSprite;
    [SerializeField] private SpriteRenderer shotgunSprite;
    [SerializeField] private float invisibilityDuration = 2f;
    [SerializeField] private float blinkInterval = 0.1f;
    
    // Start is called before the first frame update
    void Start()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
        
    }

    // Update is called once per frame
    void Update()
    {

        CheckPlayerState();
        if (isDashing || !canMove)
        {
            return;
        }
        move = Input.GetAxis("Horizontal");
        look_updown = Input.GetAxis("Vertical");

        
        Flip();
        CheckTerrain();
        
        body.velocity = new Vector2(move * moveSpeed, body.velocity.y);


        Jump();
            
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }


    }
    private void CheckPlayerState()
    {
        CheckJumping = false;
        CheckFalling = false;
        if(isGrounded)
        {
            CheckJumping = false;
            CheckFalling = false;
        }
        else
        {
            
            if (body.velocity.y < 0)
            {
                CheckFalling=true;
            }
            else if (body.velocity.y > 0)
            {
                CheckJumping = true;
            }
        }
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            jumpTimeCounter = jumpTime;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                body.velocity = new Vector2(body.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }

    private IEnumerator BlinkingEffect()
    {
        isInvis = true;
        float elapsedTime = 0f;
        Color catSpriteColor = catSprite.color;
        Color shotgunSpriteColor = shotgunSprite.color;

        while (elapsedTime < invisibilityDuration)
        {
            float t = Mathf.PingPong(Time.time / blinkInterval, 1);
            shotgunSpriteColor.a = Mathf.Lerp(1f, 0f, t);
            catSpriteColor.a = Mathf.Lerp(1f, 0f, t); 
            catSprite.color = catSpriteColor;
            shotgunSprite.color = shotgunSpriteColor; 

            yield return null; 

            elapsedTime += Time.deltaTime;
        }

        shotgunSpriteColor.a = 1f;
        catSpriteColor.a = 1f;
        shotgunSprite.color = shotgunSpriteColor;
        catSprite.color = catSpriteColor;
        isInvis = false;
    }

    private IEnumerator KnockBack()
    {
        isTakeDmg = true;
        canMove = false;
        if(isFacingRight)
        {
            body.velocity = new Vector2(-5f, 5f);
        }
        else
        {
            body.velocity = new Vector2(5f, 5f);
        }
        
        yield return new WaitForSeconds(0.5f);
        canMove = true;
        isTakeDmg= false;
    }
    private void TakeDmg()
    {
        StartCoroutine(KnockBack());
        StartCoroutine(BlinkingEffect());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("BulletEnemy"))
        {
            if(!isInvis)
            {
                cameraShake.Shake();
                TakeDmg();
            }
            
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
            canDash = true;
        }
        
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground")  )
        {
            isGrounded = false;
        }
    }
    private void Flip()
    {
        if(isFacingRight && move < 0 || move > 0 && !isFacingRight )
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }
    public void DisableMovement()
    {
        canMove = false;
        body.velocity = Vector2.zero;
    }

    public void EnableMovement()
    {
        canMove = true;
    }
    private void FixedUpdate()
    {
        if (isDashing || !canMove)
        {
            return;
        }

        
        body.velocity = new Vector2(move * moveSpeed, body.velocity.y);
        
        
    
    }
    private void CheckTerrain()
    {
        CheckGround();
        CheckWall();
        if (isTouchingWall && !isGrounded)
        {
            isGrounded = false;
        }
        else if (isTouchingWall && isGrounded)
        {
            isGrounded = true;
            
        }
    }
    private void CheckGround()
    {
        Vector2 position = (Vector2)transform.position + RayGroundOffset;
        Vector2 direction = Vector2.right;

        isGrounded = Physics2D.Raycast(position, direction, groundCheckDistance, whatIsGround);
        Debug.DrawRay(position, direction * groundCheckDistance, Color.red);
    }
    private void CheckWall()
    {
        Vector2 position = transform.position;
        Vector2 directionLeft = Vector2.left;
        Vector2 directionRight = Vector2.right;

        
        bool isTouchingWallLeft = Physics2D.Raycast(position, directionLeft, wallCheckDistance, whatIsGround);
        bool isTouchingWallRight = Physics2D.Raycast(position, directionRight, wallCheckDistance, whatIsGround);

        
        isTouchingWall =  isTouchingWallLeft || isTouchingWallRight;
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = body.gravityScale;

        body.gravityScale = 0f;
        body.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        StartCoroutine(SpawnAfterimages());
        if (cameraShake != null)
        {
            cameraShake.Shake();
        }
        yield return new WaitForSeconds(dashingTime);

        body.gravityScale = originalGravity;     

        if (isGrounded)
        {
            canDash = true;
        }
        else
        {
            canDash = false;
        }
        isDashing = false;
    }
    private IEnumerator SpawnAfterimages()
    {
        while (isDashing)
        {

            GameObject afterimage = Instantiate(afterimagePrefab, transform.position, transform.rotation);
            if (!isFacingRight)
            {
                Vector3 localScale = afterimage.transform.localScale;
                localScale.x *= -1;
                afterimage.transform.localScale = localScale;
            }
            Destroy(afterimage, 0.15f);

            yield return new WaitForSeconds(afterimageSpawnInterval);
        }
    }
    //not use

}
