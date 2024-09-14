using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera[] _allCameras;
    private CinemachineVirtualCamera vcam;
    private CinemachineBasicMultiChannelPerlin noise;
    private float shakeDuration = 0f;
    private float shakeAmplitude = 1f;
    private float shakeFrequency = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
        UpdateActiveCamera();
    }
    private void Update()
    {
        
    
    }

    public void UpdateActiveCamera()
    {
        vcam = CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera as CinemachineVirtualCamera;
        if (vcam != null)
        {
            
            noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            
            if (noise != null)
            {
                noise.m_AmplitudeGain = 0f;
                noise.m_FrequencyGain = 0f;
            }
        }
    }
    public void Shake()
    {
        
        shakeAmplitude = 1.1f;
        shakeFrequency = 1.1f;
        shakeDuration = 0.2f;

        noise.m_AmplitudeGain = shakeAmplitude;
        noise.m_FrequencyGain = shakeFrequency;

        StartCoroutine(StopShakeAfterDuration());
    }

    private IEnumerator StopShakeAfterDuration()
    {
        yield return new WaitForSeconds(shakeDuration);
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
        
    }
}
