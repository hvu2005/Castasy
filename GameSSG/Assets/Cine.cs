using Cinemachine;
using UnityEngine;

public class Cine : MonoBehaviour
{
    [SerializeField] private Cat cat;
    [SerializeField] private CinemachineVirtualCamera vcam;
    private CinemachineFramingTransposer framingTransposer;
    [SerializeField] private Vector3 maxOffsetLeft = new Vector3(-2, 1, -10);
    [SerializeField] private Vector3 maxOffsetRight = new Vector3(2, 1, -10);
    [SerializeField] private float transitionDuration = 1f;
    private Vector3 currentOffset;
    private float currentTimeHorizontal;

    
    // Start is called before the first frame update
    void Start()
    {
        framingTransposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        currentOffset = framingTransposer.m_TrackedObjectOffset;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (framingTransposer == null) return;

        TurningCamHorizontal();
        
    }
   
    private void TurningCamHorizontal()
    {
        if (cat.isFacingRight)
        {
            currentTimeHorizontal += Time.deltaTime / transitionDuration;
            framingTransposer.m_TrackedObjectOffset = Vector3.Lerp(currentOffset, maxOffsetRight, currentTimeHorizontal);
        }
        else
        {
            currentTimeHorizontal += Time.deltaTime / transitionDuration;
            framingTransposer.m_TrackedObjectOffset = Vector3.Lerp(currentOffset, maxOffsetLeft, currentTimeHorizontal);
        }


        if ((cat.isFacingRight && framingTransposer.m_TrackedObjectOffset == maxOffsetRight) ||
            (!cat.isFacingRight && framingTransposer.m_TrackedObjectOffset == maxOffsetLeft))
        {
            currentTimeHorizontal = 0f;
            currentOffset = framingTransposer.m_TrackedObjectOffset;
        }
    }
}
