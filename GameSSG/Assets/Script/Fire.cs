using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Fire : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Bullet;
    public Cat cat;
    public Transform target;
    public Vector3 offSetRight;
    public Vector3 offSetLeft;
    public Vector3 offSetRightDown;
    public Vector3 offSetLeftDown;
    
    public float reloadTime = 1f;
    public int CurrentBullet = 2;
    private bool isReloading = false;
    public bool isShooting = false;
    public CameraShake cameraShake;

    //QuanTinh
    public Vector3 QuanTinhRight;
    public Vector3 QuanTinhLeft;
    public float QuanTinhUp;
    public float QuanTinhDown;
    public float shootingDelay;
    //
    
    void Start()
    {
        cat = GameObject.FindGameObjectWithTag("Player").GetComponent<Cat>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (cat.isFacingRight)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        if (Input.GetMouseButtonDown(0) && !cat.isDashing && CurrentBullet > 0)
        {
            StartCoroutine(Shoot());
            
        }
        

        if (CurrentBullet < 2 && !isReloading && cat.isGrounded)
        {
            if(isReloading)
            {
                return;
            }
            StartCoroutine(Reload());
        }
        
        BulletPosition();
    }
    
    private IEnumerator Shoot()
    {
        isShooting = true;
        
        CurrentBullet--;
        Instantiate(Bullet, transform.position, transform.rotation);
        if (cameraShake != null)
        {
            cameraShake.Shake();
        }
        StartCoroutine(QuanTinhDelay());
        
        yield return new WaitForSeconds(0f);
        isShooting = false;

    }
    
    
    private void BulletPosition()
    {
        if (cat.look_updown > 0)
        {
            if (cat.isFacingRight)
            {
                transform.position = target.position + offSetRight;
                transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            }
            else
            {
                transform.position = target.position + offSetLeft;
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            }





        }
        else if (cat.look_updown < 0 && !cat.isGrounded)
        {
            if (cat.isFacingRight)
            {
                transform.position = target.position + offSetRightDown;
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                transform.position = target.position + offSetLeftDown;
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
        else
        {
            if(cat.isFacingRight)
            {
                transform.position = target.position + new Vector3(0.5f, 0.25f, 0f);
            }
            else
            {
                transform.position = target.position + new Vector3(-0.5f,0.25f,0f);
            }
            
            transform.rotation = Quaternion.identity;
        }
    }
    private IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        
          CurrentBullet = 2;
        
        
        isReloading = false;
    }
    private void QuanTinh()
    {
        if(cat.isFacingRight && cat.look_updown == 0)
        {

            cat.transform.position += QuanTinhRight;
        }
        else if(!cat.isFacingRight && cat.look_updown == 0)
        {

            cat.transform.position += QuanTinhLeft;
        } 
        else if (cat.look_updown > 0 && !cat.isGrounded) 
        {
            cat.body.velocity = new Vector2(cat.body.velocity.x, QuanTinhUp);
        }
        else if(cat.look_updown < 0 && !cat.isGrounded)
        {
            cat.body.velocity = new Vector2(cat.body.velocity.x, QuanTinhDown);
        }
    }
    private IEnumerator QuanTinhDelay()
    {

        QuanTinh();
        yield return new WaitForSeconds(0f);
        
    }
}
