using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera cameraFromLeft;
    public CinemachineVirtualCamera cameraFromRight;
    private CinemachineVirtualCamera currentCamera;
    [SerializeField] private Cat cat;

    private void OnTriggerExit2D(Collider2D other)
    {
        if(currentCamera == null)
        {
            if (cat.body.velocity.x > 0f)
            {
                currentCamera = cameraFromLeft;
            }
            else if (cat.body.velocity.x < 0f)
            {
                currentCamera = cameraFromRight;
            }
        }
        if (other.CompareTag("Player")) 
        {
            
            SwapCam(cameraFromLeft, cameraFromRight);
        }
    }
 
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void SwapCam(CinemachineVirtualCamera left,  CinemachineVirtualCamera right)
    {
        if (currentCamera == left && cat.body.velocity.x > 0f)
        {
            currentCamera = right;
            cameraFromRight.Priority = 10;
            cameraFromLeft.Priority = 0;
        }
        else if (currentCamera == right && cat.body.velocity.x < 0f)
        {
            currentCamera = left;
            cameraFromRight.Priority = 0;
            cameraFromLeft.Priority = 10;
        }
    }
}
