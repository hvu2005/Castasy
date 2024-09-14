using UnityEngine;
using Cinemachine;
using System.Collections;
public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private CinemachineVirtualCamera[] _allVirtualCameras;
    private CinemachineVirtualCamera _currentVirtualCamera;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
     
    }

   // public void SwapCamera(CinemachineVirtualCamera cameraFromLeft, CinemachineVirtualCamera cameraFromRight, Vector2 trigger)
    //{
        

      //  if (_currentVirtualCamera == cameraFromLeft && trigger.x > 0f)
     //   {
      //      Debug.Log("buh buh");
     //       SetCameraPriority(cameraFromRight);
     //   }
     //   else if (_currentVirtualCamera == cameraFromRight && trigger.x < 0f)
    //    {
    //        Debug.Log("sdfsadsf");
    //        SetCameraPriority(cameraFromLeft);
   //     }
  //  }

  //  private void SetCameraPriority(CinemachineVirtualCamera newCamera)
  //  {
  //      foreach (var camera in _allVirtualCameras)
  //      {
  //          camera.Priority = (camera == newCamera) ? 10 : 0;
    //    }
   //     _currentVirtualCamera = newCamera;
    //}
}





    // Start is called before the first frame updat
    /*
    
    [SerializeField] private CinemachineVirtualCamera[] _allVirtualCameras;

    public static CameraManager instance;
    [SerializeField] private float _fallPanAmount = 0.25f;
    [SerializeField] private float _fallYPanTime = 0.35f;
    public float _fallSpeedYDampingChangeThreshold;

    private Coroutine _lerpYPanCoroutine;

    public bool IsLerpingYDamping { get; private set; }
    public bool LerpedFromPlayerFalling {  get; set; }

    private CinemachineVirtualCamera _currentCamera;
    private CinemachineFramingTransposer _framingTransposer;
    private float _normYPanAmount;
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        for(int i =0; i< _allVirtualCameras.Length; i++)
        {
            if (_allVirtualCameras[i].enabled)
            {
                _currentCamera = _allVirtualCameras[i];

                _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }
    }
    #region Lerp the Y Damping

    public void LerpYDamping(bool isPlayerFalling)
    {
        _lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
    }
    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        IsLerpingYDamping = true;

        float startDampAmount = _framingTransposer.m_YDamping;
        float endDampAmount = 0f;

        if(isPlayerFalling)
        {
            endDampAmount = _fallPanAmount;
            LerpedFromPlayerFalling = true;
        }
        else
        {
            endDampAmount = _normYPanAmount;
        }

        //lerp the pan amount
        float elapsed = 0f;
        while (elapsed < _fallYPanTime)
        {
            elapsed += Time.deltaTime;
            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, elapsed/_fallYPanTime);

            yield return null;
        }
        IsLerpingYDamping = false;
    }
    #endregion

    */


