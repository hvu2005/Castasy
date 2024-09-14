using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Wizard : MonoBehaviour
{
    private Transform player;
    [SerializeField] private float speed;
    [SerializeField] private float lineOfSight;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float shootRange;
    [SerializeField] private float heightOffset;
    [SerializeField] private float horizontalOffset;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject fireBullet;
    [SerializeField] private float fireRate;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer noitice;
    [SerializeField] private int hp;
    [SerializeField] private DmgFlash _dmgFlash;

    private Cat cat;
    private float nextFireTime;
    private bool isMoving = true;
    private bool isScaling = true;
    void Start()
    {
        cat = GameObject.FindGameObjectWithTag("Player").GetComponent<Cat>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        noitice.transform.localScale = Vector3.zero;
        
    }

    void Update()
    {
        
        if(isMoving)
        {
            MoveBehavior();
        }
        if(hp <= 0)
        {
            StartCoroutine(Dying());
        }
    }
    private void MoveBehavior()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        Debug.Log("Distance from Player: " + distanceFromPlayer);
        if (distanceFromPlayer < lineOfSight )
        {
            if(isScaling)
            {
                StartCoroutine(ScaleNoitice());
            }
            
        }
        if (distanceFromPlayer < lineOfSight && distanceFromPlayer > shootRange)
        {
            Vector2 targetPosition = new Vector2(
                player.position.x + horizontalOffset * Mathf.Sign(transform.position.x - player.position.x),
                player.position.y + heightOffset);

            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            rb.velocity = Vector2.Lerp(rb.velocity, direction * speed, Time.deltaTime * 5f);


        }
        else if (distanceFromPlayer <= shootRange && nextFireTime < Time.time && IsPlayerVisible())
        {

            rb.velocity = Vector2.zero;
            StartCoroutine(ShootRoutine());
            nextFireTime = Time.time + fireRate;
        }
        else if(distanceFromPlayer > lineOfSight) 
        {    
            isScaling = true;
            noitice.transform.localScale = Vector3.zero;
            noitice.enabled = false;
            rb.velocity = Vector2.zero;
        }

        Flip();
    }
    private IEnumerator ScaleNoitice()
    {
        isScaling = false;
        float timeElapsed = 0f;
        noitice.enabled = true;
        while (timeElapsed < 0.1f)
        {
            noitice.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timeElapsed / 0.1f);
            timeElapsed += Time.deltaTime;
            yield return null;

        }

        noitice.transform.localScale = Vector3.one;

    }
    private IEnumerator ShootRoutine()
    {

        animator.SetBool("isShooting", true);
        yield return new WaitForSeconds(2f);
        
        Instantiate(bullet, fireBullet.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        Instantiate(bullet, fireBullet.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        Instantiate(bullet, fireBullet.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForSeconds(2f);
        animator.SetBool("isShooting", false);

    }
    private bool IsPlayerVisible()
    {
        Vector2 directionToPlayer = player.position - transform.position;
        int mask = ~LayerMask.GetMask("Camera");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, lineOfSight, mask);
        Debug.DrawRay(transform.position, directionToPlayer.normalized * lineOfSight, Color.red);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            return true;
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bullet"))
        {
            if (collision.CompareTag("Bullet"))
            {
                if (cat.isFacingRight)
                {

                    transform.position += new Vector3(0.2f, 0f, 0f);
                }
                else if (!cat.isFacingRight)
                {

                    transform.position += new Vector3(-0.2f, 0f, 0f);
                }
                else if (cat.look_updown > 0)
                {
                    transform.position += new Vector3(0f, 0.2f, 0f);
                }
                else if (cat.look_updown < 0)
                {
                    transform.position += new Vector3(0f, -0.2f, 0f);
                }

                hp--;
                _dmgFlash.CallDmgFlash();
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
        Gizmos.DrawWireSphere(transform.position, shootRange );
    }

    private void Flip()
    {
        if (transform.position.x > player.transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }
    private IEnumerator Dying()
    {
        noitice.enabled = false;
        animator.SetBool("isDead", true);
        isMoving = false;
        rb.velocity = Vector2.zero;
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);

    }
}