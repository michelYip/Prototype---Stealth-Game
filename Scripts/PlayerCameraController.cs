using UnityEngine;
using Cinemachine;

public class PlayerCameraController : MonoBehaviour
{
    #region Exposed

    [SerializeField] private CinemachineFreeLook m_locomotionCamera;
    [SerializeField] private CinemachineFreeLook m_crouchCamera;
    [SerializeField] private CinemachineFreeLook m_sprintCamera;
    [SerializeField] private CinemachineFreeLook m_slideCamera;

    #endregion

    #region Unity API
    void Awake()
    {
        _currentCamera = m_locomotionCamera;
    }

    void Update()
    {
        
    }
    #endregion

    #region Main Methods

    public void SwitchCamera(string cameraName)
	{
        
		switch (cameraName)
		{
            case "LOCOMOTION":
                m_locomotionCamera.m_XAxis.Value = _currentCamera.m_XAxis.Value;
                m_locomotionCamera.m_YAxis.Value = _currentCamera.m_YAxis.Value;
                _currentCamera = m_locomotionCamera;
                m_locomotionCamera.m_Priority = 10;
                m_crouchCamera.m_Priority = 1;
                m_sprintCamera.m_Priority = 1;
                m_slideCamera.m_Priority = 1;
                break;
            case "CROUCH":
                m_crouchCamera.m_XAxis.Value = _currentCamera.m_XAxis.Value;
                m_crouchCamera.m_YAxis.Value = _currentCamera.m_YAxis.Value;
                _currentCamera = m_crouchCamera;
                m_locomotionCamera.m_Priority = 1;
                m_crouchCamera.m_Priority = 10;
                m_sprintCamera.m_Priority = 1;
                m_slideCamera.m_Priority = 1;
                break;
            case "SPRINT":
                m_sprintCamera.m_XAxis.Value = _currentCamera.m_XAxis.Value;
                m_sprintCamera.m_YAxis.Value = _currentCamera.m_YAxis.Value;
                _currentCamera = m_sprintCamera;
                m_locomotionCamera.m_Priority = 1;
                m_crouchCamera.m_Priority = 1;
                m_sprintCamera.m_Priority = 10;
                m_slideCamera.m_Priority = 1;
                break;
            case "SLIDE":
                m_slideCamera.m_XAxis.Value = _currentCamera.m_XAxis.Value;
                m_slideCamera.m_YAxis.Value = _currentCamera.m_YAxis.Value;
                _currentCamera = m_slideCamera;
                m_locomotionCamera.m_Priority = 1;
                m_crouchCamera.m_Priority = 1;
                m_sprintCamera.m_Priority = 1;
                m_slideCamera.m_Priority = 10;
                break;
            default:
                Debug.Log("Camera is undefined");
                Debug.Break();
                break;
		}
	}

    #endregion

    #region Privates

    CinemachineFreeLook _currentCamera;

    #endregion
}
