using UnityEngine;

public class PlayerWallrunState : MonoBehaviour, StateInterface
{
    #region Exposed

    [SerializeField] private Vector3 m_colliderCenterPosition = new Vector3(0, 0.3f, 0);

    [Range(7.5f, 25f)]
    [SerializeField] private float m_wallRunSpeed = 8.5f;
    [SerializeField] private float m_upwardElevation = 1f;

    #endregion

    #region Unity API
    void Awake()
    {
        _controller = GetComponent<CharacterMovementController>();
        _camera = GetComponent<PlayerCameraController>();
    }

    void Update()
    {
    }
    #endregion

    #region Main Methods

    public void DoInit()
    {
        _controller.Velocity = Vector3.zero;

        _camera.SwitchCamera("SPRINT");
        _rightWallPosition = _controller.FindRightWall();
        _leftWallPosition = _controller.FindLeftWall();
        if (_rightWallPosition != Vector3.zero && _leftWallPosition == Vector3.zero)
		{
            _isRightWall = true;
        }
        if (_leftWallPosition != Vector3.zero && _rightWallPosition == Vector3.zero)
        {
            _isRightWall = false;
        }
        _controller.SetBool("IsWallRunning", true);
        _controller.SetBool("IsSideWallRight", _isRightWall);
        _controller.IsWallRunning = true;
    }

    public void DoUpdate()
    {
        _controller.AdjustCollider(m_colliderCenterPosition, 1.4f, 0.05f);
		if (_isRightWall)
        {
            forwardDir = Quaternion.Euler(0, 90, 0) * _controller.GetRightWallNormal();
            transform.forward = forwardDir;
            _rightWallPosition = _controller.FindRightWall();
            _controller.LerpTransformPosition(_rightWallPosition + new Vector3(-0.25f, 0, 0), 0.5f);
        }
		else
        {
            forwardDir = Quaternion.Euler(0, -90, 0) * _controller.GetLeftWallNormal();
            transform.forward = forwardDir;
            _leftWallPosition = _controller.FindLeftWall();
            _controller.LerpTransformPosition(_leftWallPosition + new Vector3(0.25f, 0, 0), 0.5f);
        }

        _controller.Velocity = forwardDir * m_wallRunSpeed + Vector3.up * m_upwardElevation;

        if (_controller.FindLeftWall() == Vector3.zero && _controller.FindRightWall() == Vector3.zero)
            _controller.IsWallRunning = false;
    }

    public void DoExit()
    {
        _controller.SetBool("IsWallRunning", false);
    }

    #endregion

    #region Privates

    private CharacterMovementController _controller;
    private PlayerCameraController _camera;

    private Vector3 _rightWallPosition = Vector3.zero;
    private Vector3 _leftWallPosition = Vector3.zero;
    private bool _isRightWall = false;

    private Vector3 forwardDir;

    #endregion
}