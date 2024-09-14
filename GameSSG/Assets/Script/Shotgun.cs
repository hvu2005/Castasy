using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    private Cat cat;
    public Transform target;
    public Vector3 offSetRight;
    public Vector3 offSetLeft;
    public Vector3 offSetRightDown;
    public Vector3 offSetLeftDown;

    [SerializeField] public Animator animator;
    [SerializeField] private Fire fire;
    [SerializeField] private float QuanTinhSungTime;

    void Start()
    {
        cat = GameObject.FindGameObjectWithTag("Player").GetComponent<Cat>();

    }

    // Update is called once per frame
    void Update()
    {
        HandleShoot();
        CatPosition();
        
    }
    private void CatPosition()
    {
        if (cat.look_updown > 0)
        {
            if (cat.isFacingRight)
            {
                transform.position = target.position + offSetRight;
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            }
            else
            {
                transform.position = target.position + offSetLeft;
                transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            }


        }
        else if (cat.look_updown < 0 && !cat.isGrounded)
        {
            if (cat.isFacingRight)
            {
                transform.position = target.position + offSetRightDown;
                transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            }
            else
            {
                transform.position = target.position + offSetLeftDown;
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            }
        }
        else
        {
            transform.position = target.position;
            transform.rotation = Quaternion.identity;
        }
    }
    
    private void HandleShoot()
    {
        if (fire.isShooting)
        {
            
            StartCoroutine(ShootingAnimation());
            
        }
    }
    private IEnumerator ShootingAnimation()
    {
        
        animator.SetBool("isFire",true);
        yield return new WaitForSeconds(QuanTinhSungTime);
        animator.SetBool("isFire", false);
    }
}

