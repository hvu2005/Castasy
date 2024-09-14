using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet1 : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Fire fire;
    [SerializeField] private Animator animator;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(fire.CurrentBullet == 0 )
        {
            animator.SetBool("isBlankBullet1", true);
        }
        else
        {
            animator.SetBool("isBlankBullet1", false);
        }
    }
}
