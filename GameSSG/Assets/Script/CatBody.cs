using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CatBody : MonoBehaviour
{
    // Start is called before the first frame update
    public Cat cat;
    public Animator animator;
    public Fire fire;
    public GameObject shotgun;
    
    void Start()
    {
        cat = GameObject.FindGameObjectWithTag("Player").GetComponent<Cat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator == null)
        {
            return;
        }

        animator.SetFloat("Speed", Mathf.Abs(cat.move));
        if (cat.isDashing)
        {
            animator.SetBool("isDash", true);
        }
        else if (!cat.isDashing)
        {
            animator.SetBool("isDash", false);
        }

        if (cat.isGrounded)
        {
            animator.SetBool("isJump", false);
        }
        else if(!cat.isGrounded)
        {
            animator.SetBool("isJump", true);
           
        }

        if (cat.look_updown > 0)
        {
            
            animator.SetBool("lookUp", true);
            animator.SetBool("lookDown", false);
            
            
        }
        else if (cat.look_updown < 0)
        {
            
            animator.SetBool("lookUp", false);
            animator.SetBool("lookDown", true);
        }
        else
        {
            animator.SetBool("lookUp", false);
            animator.SetBool("lookDown", false);

        }
        Falling();
        TakeDmgAnimation();
    }
    private void TakeDmgAnimation()
    {
        if (cat.isTakeDmg)
        {
            animator.SetBool("isTakeDmg", true);
        }
        else
        {
            animator.SetBool("isTakeDmg", false);
        }
    }
    private void Falling()
    {
        if (cat.CheckFalling)
        {
            
            animator.SetBool("isFalling", true);
        }
        else
        {
            
            animator.SetBool("isFalling", false);
        }

    }
    
}
