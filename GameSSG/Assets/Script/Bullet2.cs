using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet2 : MonoBehaviour
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
        if (fire.CurrentBullet <= 1)
        {
            animator.SetBool("isBlankBullet2", true);
        }
        else
        {
            animator.SetBool("isBlankBullet2", false);
        }
    }
}

